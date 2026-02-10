namespace Hiberus.Industria.Templates.Aspire.React.Server.Domain.Common.Errors.Validation;

/// <summary>
/// Represents a single validation failure.
/// </summary>
public class ValidationFailure
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ValidationFailure"/> class.
    /// </summary>
    /// <param name="propertyName">The name of the property that failed validation.</param>
    /// <param name="errorMessage">The validation error message.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="propertyName"/> or <paramref name="errorMessage"/> is null.</exception>
    public ValidationFailure(string propertyName, string errorMessage)
    {
        this.PropertyName = propertyName ?? throw new ArgumentNullException(nameof(propertyName));
        this.ErrorMessage = errorMessage ?? throw new ArgumentNullException(nameof(errorMessage));
    }

    /// <summary>
    /// Gets the name of the property that failed validation.
    /// </summary>
    public string PropertyName { get; }

    /// <summary>
    /// Gets the validation error message.
    /// </summary>
    public string ErrorMessage { get; }
}
