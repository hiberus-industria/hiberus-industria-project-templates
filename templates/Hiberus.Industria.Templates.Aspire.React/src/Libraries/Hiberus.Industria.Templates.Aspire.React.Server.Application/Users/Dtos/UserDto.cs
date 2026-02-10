using Hiberus.Industria.Templates.Aspire.React.Server.Domain.Users.Entities;

namespace Hiberus.Industria.Templates.Aspire.React.Server.Application.Users.Dtos;

/// <summary>
/// Represents a user data transfer object for Keycloak interactions.
/// </summary>
public class UserDto
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserDto"/> class.
    /// </summary>
    /// <param name="id">The unique identifier of the user in Keycloak.</param>
    /// <param name="username">The username of the user.</param>
    /// <param name="firstName">The first name of the user.</param>
    /// <param name="lastName">The last name of the user.</param>
    /// <param name="group">The group of the user.</param>
    /// <param name="email">The email address of the user.</param>
    public UserDto(
        long id,
        string username,
        string firstName,
        string lastName,
        string group,
        string? email
    )
    {
        this.Id = id;
        this.Username = username;
        this.FirstName = firstName;
        this.LastName = lastName;
        this.Group = group;
        this.Email = email;
    }

    /// <summary>
    /// Gets or sets the unique identifier of the user in Keycloak.
    /// </summary>
    public long Id { get; set; }

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
    public string Group { get; set; }

    /// <summary>
    /// Gets or sets the email address of the user.
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// Maps a Keycloak UserRepresentation to a UserDto.
    /// </summary>
    /// <param name="user">The Keycloak UserRepresentation to map.</param>
    /// <returns>A new instance of <see cref="UserDto"/> populated with data from the UserRepresentation.</returns>
    public static UserDto FromUser(User user)
    {
        ArgumentNullException.ThrowIfNull(user);

        return new UserDto(
            user.Id,
            user.Username,
            user.FirstName,
            user.LastName,
            user.Group,
            user.Email
        );
    }
}
