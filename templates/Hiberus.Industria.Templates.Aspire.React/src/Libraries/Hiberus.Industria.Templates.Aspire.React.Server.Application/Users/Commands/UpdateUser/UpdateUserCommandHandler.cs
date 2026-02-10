using FluentResults;
using Hiberus.Industria.Templates.Aspire.React.Server.Application.Common.Configurations;
using Hiberus.Industria.Templates.Aspire.React.Server.Application.Common.Interfaces;
using Hiberus.Industria.Templates.Aspire.React.Server.Application.Identity;
using Hiberus.Industria.Templates.Aspire.React.Server.Application.Users.Dtos;
using Hiberus.Industria.Templates.Aspire.React.Server.Application.Users.Specifications;
using Hiberus.Industria.Templates.Aspire.React.Server.Domain.Common.Errors;
using Hiberus.Industria.Templates.Aspire.React.Server.Domain.Users.Entities;
using Keycloak.AuthServices.Sdk.Admin;
using Keycloak.AuthServices.Sdk.Admin.Models;
using Keycloak.AuthServices.Sdk.Admin.Requests.Groups;
using MediatR;
using Microsoft.Extensions.Options;

namespace Hiberus.Industria.Templates.Aspire.React.Server.Application.Users.Commands.UpdateUser;

/// <summary>
/// Represents a command handler for updating a user.
/// </summary>
public sealed class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Result<UserDto>>
{
    private readonly IKeycloakClient keycloakClient;
    private readonly IOptions<KeycloakClientOptions> keycloakClientOptions;
    private readonly IRepository<User> userRepository;
    private readonly IUnitOfWork unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateUserCommandHandler"/> class.
    /// </summary>
    /// <param name="keycloakClient">The Keycloak client for user management.</param>
    /// <param name="keycloakClientOptions">Options for configuring the Keycloak client.</param>
    /// <param name="userRepository">The user repository for data access.</param>
    /// <param name="unitOfWork">The unit of work for managing transactions.</param>
    public UpdateUserCommandHandler(
        IKeycloakClient keycloakClient,
        IOptions<KeycloakClientOptions> keycloakClientOptions,
        IRepository<User> userRepository,
        IUnitOfWork unitOfWork
    )
    {
        this.keycloakClient = keycloakClient;
        this.keycloakClientOptions = keycloakClientOptions;
        this.userRepository = userRepository;
        this.unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Handles the UpdateUserCommand to update a user's information.
    /// </summary>
    /// <param name="request">The command request containing the user ID and update details.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
    /// <returns>A result containing the updated user data transfer object.</returns>
    public async Task<Result<UserDto>> Handle(
        UpdateUserCommand request,
        CancellationToken cancellationToken
    )
    {
        ArgumentNullException.ThrowIfNull(request);

        bool invalidGroup = !GroupNames.IsValidGroup(request.UserRequest.Group);
        if (invalidGroup)
        {
            NotFoundError error = new($"Group '{request.UserRequest.Group}' does not exist.");
            return Result.Fail(error);
        }

        // Check if user exists
        GetUserByIdSpecification specification = new(request.Id);
        User? user = await this
            .userRepository.FirstOrDefaultAsync(specification, cancellationToken)
            .ConfigureAwait(false);

        if (user == null)
        {
            NotFoundError error = new($"User with ID {request.Id} not found.");
            return Result.Fail(error);
        }

        string realm = this.keycloakClientOptions.Value.DestinationRealm;
        string externalId = user.ExternalId.ToString();

        // Updates the existing user group if it has changed
        Result groupChangeResult = await this.HandleUserGroupChangeAsync(
                realm,
                externalId,
                user.Group,
                request.UserRequest.Group,
                cancellationToken
            )
            .ConfigureAwait(false);

        if (groupChangeResult.IsFailed)
        {
            return Result.Fail(groupChangeResult.Errors);
        }

        // Update Keycloak user
        UserRepresentation updatedUserRepresentation = request.UserRequest.ToUserRepresentation();

        await this
            .keycloakClient.UpdateUserAsync(
                realm,
                externalId,
                updatedUserRepresentation,
                cancellationToken
            )
            .ConfigureAwait(false);

        // Update user in the database
        Result updateResult = user.Update(
            request.UserRequest.Username,
            request.UserRequest.FirstName,
            request.UserRequest.LastName,
            request.UserRequest.Group,
            request.UserRequest.Email
        );

        if (updateResult.IsFailed)
        {
            return Result.Fail(updateResult.Errors);
        }

        await this.unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        // Map updated user representation to DTO
        UserDto updatedUser = UserDto.FromUser(user);

        return Result.Ok(updatedUser);
    }

    /// <summary>
    /// Handles the change of a user's group in Keycloak.
    /// </summary>
    /// <param name="realm">The Keycloak realm.</param>
    /// <param name="externalId">The external ID of the user in Keycloak.</param>
    /// <param name="currentGroupName">The current group of the user.</param>
    /// <param name="newGroupName">The new group to assign to the user.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A result indicating success or failure of the group change operation.</returns>
    private async Task<Result> HandleUserGroupChangeAsync(
        string realm,
        string externalId,
        string currentGroupName,
        string newGroupName,
        CancellationToken cancellationToken
    )
    {
        if (string.Equals(currentGroupName, newGroupName, StringComparison.Ordinal))
        {
            return Result.Ok();
        }

        // Get current and target groups
        IEnumerable<GroupRepresentation> currentGroups = await this
            .keycloakClient.GetUserGroupsAsync(
                realm,
                externalId,
                parameters: null,
                cancellationToken
            )
            .ConfigureAwait(false);
        GetGroupsRequestParameters targetGroupParameters = new() { Search = newGroupName };
        IEnumerable<GroupRepresentation> targetGroups = await this
            .keycloakClient.GetGroupsAsync(realm, targetGroupParameters, cancellationToken)
            .ConfigureAwait(false);

        // Validate that both groups exist
        GroupRepresentation? currentGroup = currentGroups.FirstOrDefault(group =>
            string.Equals(group.Name, currentGroupName, StringComparison.Ordinal)
        );
        string? currentGroupId = currentGroup?.Id;
        if (string.IsNullOrEmpty(currentGroupId))
        {
            NotFoundError error = new(
                $"Current group '{currentGroup}' not found in external system."
            );
            return Result.Fail(error);
        }

        GroupRepresentation? targetGroup = targetGroups.FirstOrDefault(group =>
            string.Equals(group.Name, newGroupName, StringComparison.Ordinal)
        );
        string? targetGroupId = targetGroup?.Id;
        if (string.IsNullOrEmpty(targetGroupId))
        {
            NotFoundError error = new(
                $"Target group '{newGroupName}' not found in external system."
            );
            return Result.Fail(error);
        }

        // Remove user from current group
        await this
            .keycloakClient.LeaveGroupAsync(realm, externalId, currentGroupId, cancellationToken)
            .ConfigureAwait(false);
        // Add user to target group
        await this
            .keycloakClient.JoinGroupAsync(realm, externalId, targetGroupId, cancellationToken)
            .ConfigureAwait(false);

        return Result.Ok();
    }
}
