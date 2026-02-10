using FluentResults;
using Hiberus.Industria.Templates.Aspire.React.Server.Domain.Common.Errors;
using Hiberus.Industria.Templates.Aspire.React.Server.Domain.Common.Errors.Validation;
using Microsoft.AspNetCore.Mvc;

namespace Hiberus.Industria.Templates.Aspire.React.Server.Extensions;

/// <summary>
/// Extension methods for converting FluentResults to IActionResult.
/// </summary>
internal static class ResultExtensions
{
    /// <summary>
    /// Converts a FluentResults result to an IActionResult, mapping errors to ProblemDetails.
    /// </summary>
    /// <typeparam name="T">The type of the result value.</typeparam>
    /// <param name="result">The result to convert.</param>
    /// <param name="controller">The controller context for creating ProblemDetails.</param>
    /// <returns>An IActionResult representing the result.</returns>
    public static IActionResult ToActionResult<T>(this Result<T> result, ControllerBase controller)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(controller);

        if (result.IsSuccess)
        {
            return controller.Ok(result.Value);
        }

        IError? error = result.Errors.Count > 0 ? result.Errors[0] : null;
        var problemDetails = new ProblemDetails
        {
            Status = error switch
            {
                NotFoundError => StatusCodes.Status404NotFound,
                ValidationError => StatusCodes.Status400BadRequest,
                InfrastructureError => StatusCodes.Status500InternalServerError,
                _ => StatusCodes.Status400BadRequest,
            },
            Title = error?.GetType().Name ?? "Error",
            Detail = error?.Message ?? "An error occurred.",
        };

        if (error is ValidationError validationError)
        {
            problemDetails.Extensions["errors"] = validationError.Failures.Select(failure => new
            {
                failure.PropertyName,
                failure.ErrorMessage,
            });
        }
        else if (error is InfrastructureError infrastructureError)
        {
            problemDetails.Extensions["code"] = infrastructureError.Code;
        }

        return controller.Problem(
            statusCode: problemDetails.Status,
            title: problemDetails.Title,
            detail: problemDetails.Detail,
            extensions: problemDetails.Extensions
        );
    }
}
