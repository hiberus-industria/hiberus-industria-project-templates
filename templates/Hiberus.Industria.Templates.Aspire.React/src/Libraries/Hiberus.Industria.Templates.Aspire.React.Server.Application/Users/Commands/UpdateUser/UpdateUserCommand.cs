using FluentResults;
using Hiberus.Industria.Templates.Aspire.React.Server.Application.Users.Dtos;
using MediatR;

namespace Hiberus.Industria.Templates.Aspire.React.Server.Application.Users.Commands.UpdateUser;

/// <summary>
/// Command to update a user.
/// </summary>
/// <param name="Id">The ID of the user to update.</param>
/// <param name="UserRequest">The request data for the user update.</param>
public record UpdateUserCommand(long Id, UpdateUserRequestDto UserRequest)
    : IRequest<Result<UserDto>>;
