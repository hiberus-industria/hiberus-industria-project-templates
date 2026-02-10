using FluentResults;
using Hiberus.Industria.Templates.Aspire.React.Server.Application.Users.Dtos;
using MediatR;

namespace Hiberus.Industria.Templates.Aspire.React.Server.Application.Users.Commands.CreateUser;

/// <summary>
/// Command to create a user.
/// </summary>
/// <param name="UserRequest">The request data for the user creation.</param>
public record CreateUserCommand(CreateUserRequestDto UserRequest) : IRequest<Result<UserDto>>;
