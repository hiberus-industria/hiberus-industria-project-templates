using FluentResults;
using Hiberus.Industria.Templates.Aspire.React.Server.Application.Common.Configurations;
using Hiberus.Industria.Templates.Aspire.React.Server.Application.Common.Interfaces;
using Hiberus.Industria.Templates.Aspire.React.Server.Application.Users.Specifications;
using Hiberus.Industria.Templates.Aspire.React.Server.Domain.Common.Errors;
using Hiberus.Industria.Templates.Aspire.React.Server.Domain.Users.Entities;
using Keycloak.AuthServices.Sdk.Admin;
using Keycloak.AuthServices.Sdk.Admin.Models;
using MediatR;
using Microsoft.Extensions.Options;

namespace Hiberus.Industria.Templates.Aspire.React.Server.Application.Users.Commands.ResetUserPassword;

/// <summary>
/// Represents a command handler for resetting a user's password.
/// </summary>
public sealed class ResetUserPasswordCommandHandler
    : IRequestHandler<ResetUserPasswordCommand, Result<Unit>>
{
    private readonly IKeycloakClient keycloakClient;
    private readonly IOptions<KeycloakClientOptions> keycloakClientOptions;
    private readonly IReadRepository<User> userReadRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="ResetUserPasswordCommandHandler"/> class.
    /// </summary>
    /// <param name="keycloakClient">Keycloak client for user management.</param>
    /// <param name="keycloakClientOptions">Options for configuring the Keycloak client.</param>
    /// <param name="userReadRepository">User read repository for data access.</param>
    public ResetUserPasswordCommandHandler(
        IKeycloakClient keycloakClient,
        IOptions<KeycloakClientOptions> keycloakClientOptions,
        IReadRepository<User> userReadRepository
    )
    {
        this.keycloakClient = keycloakClient;
        this.keycloakClientOptions = keycloakClientOptions;
        this.userReadRepository = userReadRepository;
    }

    /// <summary>
    /// Handles the ResetUserPasswordCommand to reset a user's password by its ID.
    /// </summary>
    /// <param name="request">The command request containing the user ID.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
    /// <returns>A result indicating the success or failure of the operation.</returns>
    public async Task<Result<Unit>> Handle(
        ResetUserPasswordCommand request,
        CancellationToken cancellationToken
    )
    {
        ArgumentNullException.ThrowIfNull(request);

        // Check if user exists
        GetUserByIdSpecification specification = new(request.Id);
        User? user = await this
            .userReadRepository.FirstOrDefaultAsync(specification, cancellationToken)
            .ConfigureAwait(false);

        if (user == null)
        {
            NotFoundError error = new($"User with ID {request.Id} not found.");
            return Result.Fail(error);
        }

        // Update user credentials for Keycloak
        string? externalId = user.ExternalId.ToString();

        UserRepresentation updatedUser = new()
        {
            Id = externalId,
            Credentials =
            [
                new CredentialRepresentation
                {
                    Type = "password",
                    Value = this.keycloakClientOptions.Value.DefaultUserPassword,
                    Temporary = true,
                },
            ],
        };

        await this
            .keycloakClient.UpdateUserAsync(
                this.keycloakClientOptions.Value.DestinationRealm,
                externalId,
                updatedUser,
                cancellationToken
            )
            .ConfigureAwait(false);

        return Result.Ok();
    }
}
