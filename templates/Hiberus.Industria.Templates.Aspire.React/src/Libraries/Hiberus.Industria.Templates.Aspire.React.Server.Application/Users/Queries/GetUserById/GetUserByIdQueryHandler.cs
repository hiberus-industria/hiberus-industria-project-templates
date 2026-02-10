using FluentResults;
using Hiberus.Industria.Templates.Aspire.React.Server.Application.Common.Interfaces;
using Hiberus.Industria.Templates.Aspire.React.Server.Application.Users.Dtos;
using Hiberus.Industria.Templates.Aspire.React.Server.Application.Users.Specifications;
using Hiberus.Industria.Templates.Aspire.React.Server.Domain.Common.Errors;
using Hiberus.Industria.Templates.Aspire.React.Server.Domain.Users.Entities;
using MediatR;

namespace Hiberus.Industria.Templates.Aspire.React.Server.Application.Users.Queries.GetUserById;

/// <summary>
/// Represents a query to retrieve a user by its ID.
/// </summary>
public sealed class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, Result<UserDto>>
{
    private readonly IReadRepository<User> userReadRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetUserByIdQueryHandler"/> class.
    /// </summary>
    /// <param name="userReadRepository">User read repository for data access.</param>
    public GetUserByIdQueryHandler(IReadRepository<User> userReadRepository)
    {
        this.userReadRepository = userReadRepository;
    }

    /// <summary>
    /// Handles the GetUserByIdQuery to retrieve a user by its ID.
    /// </summary>
    /// <param name="request">The query request containing the user ID.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
    /// <returns>A result containing the user representation.</returns>
    public async Task<Result<UserDto>> Handle(
        GetUserByIdQuery request,
        CancellationToken cancellationToken
    )
    {
        ArgumentNullException.ThrowIfNull(request);

        // Check if user exists
        GetUserByIdSpecification specification = new(request.Id);
        User? user = await this
            .userReadRepository.FirstOrDefaultAsync(specification, cancellationToken)
            .ConfigureAwait(false);

        if (user == null)
        {
            NotFoundError error = new($"User with ID {request.Id} not found.");
            return Result.Fail(error);
        }

        // Map user entity to DTO
        UserDto userDto = UserDto.FromUser(user);

        return Result.Ok(userDto);
    }
}
