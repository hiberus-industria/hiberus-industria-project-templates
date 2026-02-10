using FluentResults;
using Hiberus.Industria.Templates.Aspire.React.Server.Application.Common.Interfaces;
using Hiberus.Industria.Templates.Aspire.React.Server.Application.Users.Dtos;
using Hiberus.Industria.Templates.Aspire.React.Server.Application.Users.Specifications;
using Hiberus.Industria.Templates.Aspire.React.Server.Domain.Common.Dtos;
using Hiberus.Industria.Templates.Aspire.React.Server.Domain.Users.Entities;
using MediatR;

namespace Hiberus.Industria.Templates.Aspire.React.Server.Application.Users.Queries.GetUsers;

/// <summary>
/// Represents a query to retrieve a paginated list of users.
/// </summary>
public sealed class GetUsersQueryHandler
    : IRequestHandler<GetUsersQuery, Result<PagedResultDto<UserDto>>>
{
    private readonly IReadRepository<User> userReadRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetUsersQueryHandler"/> class.
    /// </summary>
    /// <param name="userReadRepository">User read repository for data access.</param>
    public GetUsersQueryHandler(IReadRepository<User> userReadRepository)
    {
        this.userReadRepository = userReadRepository;
    }

    /// <summary>
    /// Handles the GetUsersQuery to retrieve a paginated list of users.
    /// </summary>
    /// <param name="request">The query request containing pagination parameters.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
    /// <returns>A result containing a paginated list of user representations.</returns>
    public async Task<Result<PagedResultDto<UserDto>>> Handle(
        GetUsersQuery request,
        CancellationToken cancellationToken
    )
    {
        ArgumentNullException.ThrowIfNull(request);

        // Get total count with filters (without pagination)
        GetUsersFilterSpecification filterSpecification = new(request.Group, request.Username);
        int totalCount = await this
            .userReadRepository.CountAsync(filterSpecification, cancellationToken)
            .ConfigureAwait(false);

        // Get paginated data
        GetUsersWithPaginationSpecification paginationSpecification = new(
            request.Page,
            request.PageSize,
            request.Group,
            request.Username
        );
        List<User> users = await this
            .userReadRepository.ListAsync(paginationSpecification, cancellationToken)
            .ConfigureAwait(false);

        List<UserDto> userDtos = users.ConvertAll(UserDto.FromUser);

        var result = new PagedResultDto<UserDto>(
            userDtos,
            request.Page,
            request.PageSize,
            totalCount
        );

        return Result.Ok(result);
    }
}
