namespace Hiberus.Industria.Templates.Aspire.React.Server.Domain.Common.Errors;

/// <summary>
/// Represents a domain-specific error.
/// </summary>
public class DomainError : DomainErrorBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DomainError"/> class with metadata.
    /// </summary>
    /// <param name="code">The error code.</param>
    /// <param name="message">The error message.</param>
    /// <param name="metadata">Metadata associated with the error.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="code"/>, <paramref name="message"/>, or <paramref name="metadata"/> is null.</exception>
    public DomainError(string code, string message, IDictionary<string, object> metadata)
        : base(code, message, metadata) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="DomainError"/> class with an empty metadata dictionary.
    /// </summary>
    /// <param name="code">The error code.</param>
    /// <param name="message">The error message.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="code"/> or <paramref name="message"/> is null.</exception>
    public DomainError(string code, string message)
        : base(code, message) { }
}
