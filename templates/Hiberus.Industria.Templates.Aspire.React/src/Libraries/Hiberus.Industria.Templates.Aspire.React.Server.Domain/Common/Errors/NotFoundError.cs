using FluentResults;

namespace Hiberus.Industria.Templates.Aspire.React.Server.Domain.Common.Errors;

/// <summary>
/// Represents an error indicating that a requested resource was not found.
/// </summary>
public class NotFoundError : Error
{
    /// <summary>
    /// Initializes a new instance of the <see cref="NotFoundError"/> class with a message.
    /// </summary>
    /// <param name="message">The error message describing the not found condition.</param>
    public NotFoundError(string message)
        : base(message) { }
}
