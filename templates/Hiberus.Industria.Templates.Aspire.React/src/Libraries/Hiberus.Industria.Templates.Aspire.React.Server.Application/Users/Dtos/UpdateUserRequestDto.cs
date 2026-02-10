using Hiberus.Industria.Templates.Aspire.React.Server.Application.Identity;
using Keycloak.AuthServices.Sdk.Admin.Models;

namespace Hiberus.Industria.Templates.Aspire.React.Server.Application.Users.Dtos;

/// <summary>
/// Represents the request to update the user information.
/// </summary>
public class UpdateUserRequestDto
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateUserRequestDto"/> class.
    /// </summary>
    /// <param name="username">The username of the user.</param>
    /// <param name="firstName">The first name of the user.</param>
    /// <param name="lastName">The last name of the user.</param>
    /// <param name="email">The email address of the user.</param>
    public UpdateUserRequestDto(string username, string firstName, string lastName, string? email)
    {
        this.Username = username;
        this.FirstName = firstName;
        this.LastName = lastName;
        this.Email = email;
    }

    /// <summary>
    /// Gets or sets the username of the user.
    /// </summary>
    public string Username { get; set; }

    /// <summary>
    /// Gets or sets the first name of the user.
    /// </summary>
    public string FirstName { get; set; }

    /// <summary>
    /// Gets or sets the last name of the user.
    /// </summary>
    public string LastName { get; set; }

    /// <summary>
    /// Gets or sets the group of the user.
    /// </summary>
    public string Group { get; set; } = GroupNames.Operators;

    /// <summary>
    /// Gets or sets the email address of the user.
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// Converts the current instance to a <see cref="UserRepresentation"/> object.
    /// </summary>
    /// <returns>A <see cref="UserRepresentation"/> object with the properties set from this instance.</returns>
    public UserRepresentation ToUserRepresentation()
    {
        return new UserRepresentation
        {
            Username = this.Username,
            FirstName = this.FirstName,
            LastName = this.LastName,
            Email = this.Email,
        };
    }
}
