namespace Hiberus.Industria.Templates.Aspire.React.Server.Domain.Common.Errors.Validation;

/// <summary>
/// Represents a validation error with associated failures.
/// </summary>
public class ValidationError : DomainErrorBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ValidationError"/> class.
    /// </summary>
    /// <param name="failures">The validation failures.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="failures"/> is null.</exception>
    public ValidationError(IEnumerable<ValidationFailure> failures)
        : base(
            "ValidationError",
            "One or more validation errors occurred.",
            new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)
            {
                ["ValidationFailures"] =
                    failures ?? throw new ArgumentNullException(nameof(failures)),
            }
        )
    {
        this.Failures = failures;
    }

    /// <summary>
    /// Gets the list of validation failures.
    /// </summary>
    public IEnumerable<ValidationFailure> Failures { get; }
}
