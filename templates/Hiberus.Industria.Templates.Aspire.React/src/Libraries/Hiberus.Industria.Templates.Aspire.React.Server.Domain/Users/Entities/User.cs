using System.Text.RegularExpressions;
using FluentResults;
using Hiberus.Industria.Templates.Aspire.React.Server.Domain.Common.Errors;
using Hiberus.Industria.Templates.Aspire.React.Server.Domain.Common.Interfaces;
using Hiberus.Industria.Templates.Aspire.React.Server.Domain.Users.Errors;

namespace Hiberus.Industria.Templates.Aspire.React.Server.Domain.Users.Entities;

/// <summary>
/// Represents an application user.
/// </summary>
public sealed class User : IAuditable
{
    /// <summary>
    /// Pre-compiled email regex with a timeout to mitigate potential ReDoS.
    /// </summary>
    private static readonly Regex EmailRegex = new(
        @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
        RegexOptions.CultureInvariant | RegexOptions.Compiled,
        TimeSpan.FromMilliseconds(200)
    );

    private User() { }

    /// <summary>
    /// Gets the unique identifier of the user.
    /// </summary>
    public long Id { get; private set; }

    /// <summary>
    /// Gets the external identifier of the user.
    /// </summary>
    public Guid ExternalId { get; private set; }

    /// <summary>
    /// Gets the username used for login.
    /// </summary>
    public string Username { get; private set; } = null!;

    /// <summary>
    /// Gets the user's first name.
    /// </summary>
    public string FirstName { get; private set; } = null!;

    /// <summary>
    /// Gets the user's last name.
    /// </summary>
    public string LastName { get; private set; } = null!;

    /// <summary>
    /// Gets the user's group.
    /// </summary>
    public string Group { get; private set; } = null!;

    /// <summary>
    /// Gets the user's email address.
    /// </summary>
    public string? Email { get; private set; }

    /// <summary>
    /// Gets the date and time when the entity was created.
    /// </summary>
    public DateTime CreatedAt { get; }

    /// <summary>
    /// Gets the date and time when the entity was last updated.
    /// </summary>
    public DateTime? UpdatedAt { get; }

    /// <summary>
    /// Gets the identifier of the user who created the entity.
    /// </summary>
    public string CreatedBy { get; } = null!;

    /// <summary>
    /// Gets the identifier of the user who last updated the entity.
    /// </summary>
    public string? UpdatedBy { get; }

    /// <summary>
    /// Performs a basic validation of an email address format.
    /// </summary>
    /// <param name="email">Email to validate.</param>
    /// <returns>True if the format looks valid; otherwise false.</returns>
    public static bool IsValidEmail(string? email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return true; // Allow null or empty emails
        }

        string trimmedEmail = email.Trim();
        return EmailRegex.IsMatch(trimmedEmail);
    }

    /// <summary>
    /// Creates a new user instance with the specified data.
    /// </summary>
    /// <param name="externalId">The user's external identifier.</param>
    /// <param name="username">The login username.</param>
    /// <param name="firstName">The user's first name.</param>
    /// <param name="lastName">The user's last name.</param>
    /// <param name="group">The user's group.</param>
    /// <param name="email">The user's email address.</param>
    /// <returns>A result containing the created user or a domain error if validation fails.</returns>
    public static Result<User> Create(
        Guid externalId,
        string username,
        string firstName,
        string lastName,
        string group,
        string? email
    )
    {
        ArgumentNullException.ThrowIfNull(username);
        ArgumentNullException.ThrowIfNull(firstName);
        ArgumentNullException.ThrowIfNull(lastName);
        ArgumentNullException.ThrowIfNull(group);

        if (!IsValidEmail(email))
        {
            DomainErrorBase error = UserDomainErrors.InvalidEmail(email);
            return Result.Fail(error);
        }

        var user = new User
        {
            Id = 0, // Will be set by database
            ExternalId = externalId,
            Username = username.Trim(),
            FirstName = firstName.Trim(),
            LastName = lastName.Trim(),
            Group = group.Trim(),
            Email = email?.Trim(),
        };

        return Result.Ok(user);
    }

    /// <summary>
    /// Updates mutable fields of the user aggregate.
    /// </summary>
    /// <param name="username">The new username.</param>
    /// <param name="firstName">The new first name.</param>
    /// <param name="lastName">The new last name.</param>
    /// <param name="group">The new group.</param>
    /// <param name="email">The new email.</param>
    /// <returns>A result indicating success or a domain error when validation fails.</returns>
    public Result Update(
        string username,
        string firstName,
        string lastName,
        string group,
        string? email
    )
    {
        ArgumentNullException.ThrowIfNull(username);
        ArgumentNullException.ThrowIfNull(firstName);
        ArgumentNullException.ThrowIfNull(lastName);
        ArgumentNullException.ThrowIfNull(group);

        if (!IsValidEmail(email))
        {
            DomainErrorBase error = UserDomainErrors.InvalidEmail(email);
            return Result.Fail(error);
        }

        this.Username = username.Trim();
        this.FirstName = firstName.Trim();
        this.LastName = lastName.Trim();
        this.Group = group.Trim();
        this.Email = email?.Trim();

        return Result.Ok();
    }
}
