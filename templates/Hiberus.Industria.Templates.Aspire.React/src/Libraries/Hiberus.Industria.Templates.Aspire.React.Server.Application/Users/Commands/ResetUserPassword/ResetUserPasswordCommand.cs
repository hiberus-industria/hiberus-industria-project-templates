using FluentResults;
using MediatR;

namespace Hiberus.Industria.Templates.Aspire.React.Server.Application.Users.Commands.ResetUserPassword;

/// <summary>
/// Represents a command to reset a user's password.
/// </summary>
/// <param name="Id">The unique identifier of the user.</param>
/// <returns>A request that can be handled by a command handler to reset the user's password.</returns>
public record ResetUserPasswordCommand(long Id) : IRequest<Result<Unit>>;
