using FluentResults;
using Hiberus.Industria.Templates.Aspire.React.Server.Application.Users.Dtos;
using MediatR;

namespace Hiberus.Industria.Templates.Aspire.React.Server.Application.Users.Queries.GetUserById;

/// <summary>
/// Represents a query to retrieve a user by its ID.
/// </summary>
/// <param name="Id">The unique identifier of the user.</param>
/// <returns>A request that can be handled by a query handler to return the user details.</returns>
public record GetUserByIdQuery(long Id) : IRequest<Result<UserDto>>;
