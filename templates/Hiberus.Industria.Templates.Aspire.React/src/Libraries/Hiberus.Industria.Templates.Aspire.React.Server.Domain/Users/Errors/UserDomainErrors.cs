using Hiberus.Industria.Templates.Aspire.React.Server.Domain.Common.Errors;

namespace Hiberus.Industria.Templates.Aspire.React.Server.Domain.Users.Errors;

/// <summary>
/// Defines domain-specific errors for the User.
/// </summary>
public static class UserDomainErrors
{
    /// <summary>
    /// Creates an error indicating that a user already exists.
    /// </summary>
    /// <param name="username">The username of the existing user.</param>
    /// <returns>A domain error representing the user already exists error.</returns>
    public static DomainErrorBase UserAlreadyExists(string username) =>
        new DomainError(
            code: "User.AlreadyExists",
            message: $"The user '{username}' already exists."
        );

    /// <summary>
    /// Creates an error indicating an invalid email address.
    /// </summary>
    /// <param name="email">The invalid email address.</param>
    /// <returns>A domain error representing the invalid email.</returns>
    public static DomainErrorBase InvalidEmail(string? email) =>
        new DomainError(
            code: "User.InvalidEmail",
            message: $"The email '{email ?? "N/A"}' is not valid."
        );
}
