using FluentResults;
using FluentValidation;
using Hiberus.Industria.Templates.Aspire.React.Server.Domain.Common.Errors.Validation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Hiberus.Industria.Templates.Aspire.React.Server.Application.Common.Behaviors;

/// <summary>
/// Pipeline behavior to validate requests using FluentValidation.
/// </summary>
/// <typeparam name="TRequest">The request type.</typeparam>
/// <typeparam name="TResponse">The response type.</typeparam>
public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : ResultBase, new()
{
    private readonly ILogger<ValidationBehavior<TRequest, TResponse>> logger;
    private readonly IValidator<TRequest>? validator;

    /// <summary>
    /// Initializes a new instance of the <see cref="ValidationBehavior{TRequest, TResponse}"/> class.
    /// </summary>
    /// <param name="logger">The logger for this behavior.</param>
    /// <param name="validator">The validator for the request, if any.</param>
    public ValidationBehavior(
        ILogger<ValidationBehavior<TRequest, TResponse>> logger,
        IValidator<TRequest>? validator = null
    )
    {
        this.logger = logger;
        this.validator = validator;
    }

    /// <summary>
    /// Handles validation of the request before passing it to the next handler.
    /// </summary>
    /// <param name="request">The request to validate.</param>
    /// <param name="next">The delegate to the next handler.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The response, or a failed result if validation fails.</returns>
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken
    )
    {
        ArgumentNullException.ThrowIfNull(next);

        if (this.validator == null)
        {
            if (this.logger.IsEnabled(LogLevel.Debug))
            {
                this.logger.LogDebug(
                    "No validator found for request type {RequestType}",
                    typeof(TRequest).Name
                );
            }

            return await next(cancellationToken).ConfigureAwait(false);
        }

        if (this.logger.IsEnabled(LogLevel.Debug))
        {
            this.logger.LogDebug("Validating request type {RequestType}", typeof(TRequest).Name);
        }

        FluentValidation.Results.ValidationResult validationResult = await this
            .validator.ValidateAsync(request, cancellationToken)
            .ConfigureAwait(false);
        if (validationResult.IsValid)
        {
            if (this.logger.IsEnabled(LogLevel.Debug))
            {
                this.logger.LogDebug(
                    "Validation passed for request type {RequestType}",
                    typeof(TRequest).Name
                );
            }

            return await next(cancellationToken).ConfigureAwait(false);
        }

        List<ValidationFailure> errors = validationResult.Errors.ConvertAll(
            error => new ValidationFailure(error.PropertyName, error.ErrorMessage)
        );

        if (this.logger.IsEnabled(LogLevel.Warning))
        {
            IEnumerable<string> errorMessages = errors.Select(failure =>
                $"{failure.PropertyName}: {failure.ErrorMessage}"
            );
            this.logger.LogWarning(
                "Validation failed for request type {RequestType} with errors: {Errors}",
                typeof(TRequest).Name,
                string.Join(", ", errorMessages)
            );
        }

        var result = new TResponse();
        var validationError = new ValidationError(errors);
        result.Reasons.Add(validationError);
        return result;
    }
}
