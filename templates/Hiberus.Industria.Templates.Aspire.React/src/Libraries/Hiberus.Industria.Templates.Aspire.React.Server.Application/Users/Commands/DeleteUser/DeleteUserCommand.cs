using FluentResults;
using MediatR;

namespace Hiberus.Industria.Templates.Aspire.React.Server.Application.Users.Commands.DeleteUser;

/// <summary>
/// Represents a command to delete a user.
/// </summary>
/// <param name="Id">The unique identifier of the user.</param>
/// <returns>A request that can be handled by a command handler to delete the user.</returns>
public record DeleteUserCommand(long Id) : IRequest<Result<Unit>>;
