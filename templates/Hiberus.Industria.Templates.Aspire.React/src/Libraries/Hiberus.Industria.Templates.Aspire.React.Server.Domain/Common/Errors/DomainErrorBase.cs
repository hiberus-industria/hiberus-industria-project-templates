using FluentResults;

namespace Hiberus.Industria.Templates.Aspire.React.Server.Domain.Common.Errors;

/// <summary>
/// Base class for domain errors, implementing the FluentResults IError interface.
/// </summary>
public abstract class DomainErrorBase : IError
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DomainErrorBase"/> class.
    /// </summary>
    /// <param name="code">The error code.</param>
    /// <param name="message">The error message.</param>
    /// <param name="metadata">Optional metadata associated with the error. Defaults to an empty dictionary.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="code"/> or <paramref name="message"/> is null.</exception>
    protected DomainErrorBase(string code, string message, IDictionary<string, object> metadata)
    {
        this.Code = code ?? throw new ArgumentNullException(nameof(code));
        this.Message = message ?? throw new ArgumentNullException(nameof(message));
        // Initialize as Dictionary to satisfy IReason.Metadata requirement
        this.Metadata = new Dictionary<string, object>(metadata, StringComparer.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DomainErrorBase"/> class with an empty metadata dictionary.
    /// </summary>
    /// <param name="code">The error code.</param>
    /// <param name="message">The error message.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="code"/> or <paramref name="message"/> is null.</exception>
    protected DomainErrorBase(string code, string message)
        : this(code, message, new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)) { }

    /// <summary>
    /// Gets the error code.
    /// </summary>
    public string Code { get; }

    /// <summary>
    /// Gets the error message.
    /// </summary>
    public string Message { get; }

    /// <summary>
    /// Gets the reasons for the error, if any.
    /// </summary>
    public List<IError> Reasons { get; } = [];

    /// <summary>
    /// Gets the metadata associated with the error.
    /// </summary>
    public Dictionary<string, object> Metadata { get; }
}
