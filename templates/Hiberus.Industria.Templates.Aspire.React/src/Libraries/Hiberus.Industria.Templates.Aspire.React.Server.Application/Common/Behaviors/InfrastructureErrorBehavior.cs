using FluentResults;
using Hiberus.Industria.Templates.Aspire.React.Server.Domain.Common.Errors;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Hiberus.Industria.Templates.Aspire.React.Server.Application.Common.Behaviors;

/// <summary>
/// Pipeline behavior to handle infrastructure exceptions and map them to InfrastructureError.
/// </summary>
/// <typeparam name="TRequest">The request type.</typeparam>
/// <typeparam name="TResponse">The response type.</typeparam>
public class InfrastructureErrorBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : ResultBase, new()
{
    private readonly ILogger<InfrastructureErrorBehavior<TRequest, TResponse>> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="InfrastructureErrorBehavior{TRequest, TResponse}"/> class.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    public InfrastructureErrorBehavior(
        ILogger<InfrastructureErrorBehavior<TRequest, TResponse>> logger
    )
    {
        this.logger = logger;
    }

    /// <summary>
    /// Handles infrastructure exceptions during request processing.
    /// </summary>
    /// <param name="request">The request being processed.</param>
    /// <param name="next">The delegate to the next handler.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The response, or a failed result if an infrastructure error occurs.</returns>
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken
    )
    {
        ArgumentNullException.ThrowIfNull(next);

        try
        {
            return await next(cancellationToken).ConfigureAwait(false);
        }
        catch (DbUpdateException exception)
        {
            var result = new TResponse();
            var error = new InfrastructureError(
                "Database.SaveFailed",
                "Failed to save data to the database.",
                exception
            );
            result.Reasons.Add(error);

            this.logger.LogError(exception, "An error occurred while updating the database.");

            return result;
        }
    }
}
