using FluentResults;
using Hiberus.Industria.Templates.Aspire.React.Server.Application.Common.Configurations;
using Hiberus.Industria.Templates.Aspire.React.Server.Application.Common.Interfaces;
using Hiberus.Industria.Templates.Aspire.React.Server.Application.Identity;
using Hiberus.Industria.Templates.Aspire.React.Server.Application.Users.Dtos;
using Hiberus.Industria.Templates.Aspire.React.Server.Application.Users.Specifications;
using Hiberus.Industria.Templates.Aspire.React.Server.Domain.Common.Errors;
using Hiberus.Industria.Templates.Aspire.React.Server.Domain.Users.Entities;
using Hiberus.Industria.Templates.Aspire.React.Server.Domain.Users.Errors;
using Keycloak.AuthServices.Sdk.Admin;
using Keycloak.AuthServices.Sdk.Admin.Models;
using MediatR;
using Microsoft.Extensions.Options;

namespace Hiberus.Industria.Templates.Aspire.React.Server.Application.Users.Commands.CreateUser;

/// <summary>
/// Handles the creation of a new user.
/// </summary>
public sealed class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Result<UserDto>>
{
    private readonly IKeycloakClient keycloakClient;
    private readonly IOptions<KeycloakClientOptions> keycloakClientOptions;
    private readonly IRepository<User> userRepository;
    private readonly IUnitOfWork unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateUserCommandHandler"/> class.
    /// </summary>
    /// <param name="keycloakClient">Keycloak client for user management.</param>
    /// <param name="keycloakClientOptions">Options for configuring the Keycloak client.</param>
    /// <param name="userRepository">User repository for data access.</param>
    /// <param name="unitOfWork">The unit of work for managing transactions.</param>
    public CreateUserCommandHandler(
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
    /// Handles the creation of a new user.
    /// </summary>
    /// <param name="request">The command request containing the user data.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
    /// <returns>A result containing the created user representation.</returns>
    public async Task<Result<UserDto>> Handle(
        CreateUserCommand request,
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

        KeycloakClientOptions options = this.keycloakClientOptions.Value;

        // Check if user already exists
        GetUserByUsernameSpecification specification = new(request.UserRequest.Username);
        User? existingUser = await this
            .userRepository.FirstOrDefaultAsync(specification, cancellationToken)
            .ConfigureAwait(false);

        if (existingUser != null)
        {
            DomainErrorBase error = UserDomainErrors.UserAlreadyExists(existingUser.Username);
            return Result.Fail(error);
        }

        CreateUserRequestDto userRequest = request.UserRequest;

        // Create Keycloak user
        UserRepresentation newKeycloakUserRepresentation = userRequest.ToUserRepresentation(
            options.DefaultUserPassword
        );

        HttpResponseMessage keycloakResponse = await this
            .keycloakClient.CreateUserWithResponseAsync(
                options.DestinationRealm,
                newKeycloakUserRepresentation,
                cancellationToken
            )
            .ConfigureAwait(false);

        // Keycloak does not return the user ID in the response body, so we need to extract it from the Location header
        string? keycloakResponseUserId = keycloakResponse
            .Headers.Location?.Segments.LastOrDefault()
            ?.TrimEnd('/');

        // Create the user in the database
        if (!Guid.TryParse(keycloakResponseUserId, out Guid externalId))
        {
            InfrastructureError error = new(
                "Keycloak.UserId.InvalidFormat",
                "Invalid Keycloak user ID format."
            );
            return Result.Fail(error);
        }

        Result<User> createUserResult = User.Create(
            externalId,
            userRequest.Username,
            userRequest.FirstName,
            userRequest.LastName,
            userRequest.Group,
            userRequest.Email
        );

        if (createUserResult.IsFailed)
        {
            return Result.Fail(createUserResult.Errors);
        }

        User user = createUserResult.Value;

        await this.userRepository.AddAsync(user, cancellationToken).ConfigureAwait(false);
        await this.unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        // Map the created user to a DTO
        UserDto userDto = UserDto.FromUser(user);

        return Result.Ok(userDto);
    }
}
