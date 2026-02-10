using FluentResults;
using Hiberus.Industria.Templates.Aspire.React.Server.Application.Common.Configurations;
using Hiberus.Industria.Templates.Aspire.React.Server.Application.Common.Interfaces;
using Hiberus.Industria.Templates.Aspire.React.Server.Application.Users.Specifications;
using Hiberus.Industria.Templates.Aspire.React.Server.Domain.Common.Errors;
using Hiberus.Industria.Templates.Aspire.React.Server.Domain.Users.Entities;
using Keycloak.AuthServices.Sdk.Admin;
using MediatR;
using Microsoft.Extensions.Options;

namespace Hiberus.Industria.Templates.Aspire.React.Server.Application.Users.Commands.DeleteUser;

/// <summary>
/// Represents a command handler for deleting a user.
/// </summary>
public sealed class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Result<Unit>>
{
    private readonly IKeycloakClient keycloakClient;
    private readonly IOptions<KeycloakClientOptions> keycloakClientOptions;
    private readonly IRepository<User> userRepository;
    private readonly IUnitOfWork unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteUserCommandHandler"/> class.
    /// </summary>
    /// <param name="keycloakClient">Keycloak client for user management.</param>
    /// <param name="keycloakClientOptions">Options for configuring the Keycloak client.</param>
    /// <param name="userRepository">User repository for data access.</param>
    /// <param name="unitOfWork">The unit of work for managing transactions.</param>
    public DeleteUserCommandHandler(
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
    /// Handles the DeleteUserCommand to delete a user by its ID.
    /// </summary>
    /// <param name="request">The command request containing the user ID.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
    /// <returns>A result indicating the success or failure of the operation.</returns>
    public async Task<Result<Unit>> Handle(
        DeleteUserCommand request,
        CancellationToken cancellationToken
    )
    {
        ArgumentNullException.ThrowIfNull(request);

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

        // Delete user from Keycloak
        string? externalId = user.ExternalId.ToString();
        await this
            .keycloakClient.DeleteUserAsync(
                this.keycloakClientOptions.Value.DestinationRealm,
                externalId,
                cancellationToken
            )
            .ConfigureAwait(false);

        // Delete user from the database
        await this.userRepository.DeleteAsync(user, cancellationToken).ConfigureAwait(false);
        await this.unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        return Result.Ok();
    }
}
