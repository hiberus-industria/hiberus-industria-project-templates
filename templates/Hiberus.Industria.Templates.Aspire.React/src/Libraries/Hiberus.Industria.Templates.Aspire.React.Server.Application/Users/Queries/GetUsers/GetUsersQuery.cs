using FluentResults;
using Hiberus.Industria.Templates.Aspire.React.Server.Application.Users.Dtos;
using Hiberus.Industria.Templates.Aspire.React.Server.Domain.Common.Dtos;
using MediatR;

namespace Hiberus.Industria.Templates.Aspire.React.Server.Application.Users.Queries.GetUsers;

/// <summary>
/// Represents a query to retrieve a paginated list of users.
/// </summary>
public class GetUsersQuery : IRequest<Result<PagedResultDto<UserDto>>>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetUsersQuery"/> class with default values.
    /// </summary>
    public GetUsersQuery() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="GetUsersQuery"/> class with specified pagination parameters.
    /// </summary>
    /// <param name="page">The page number to retrieve (must be greater than 0).</param>
    /// <param name="pageSize">The number of items per page (must be greater than 0).</param>
    /// <param name="group">The optional group filters to apply when retrieving users.</param>
    /// <param name="username">An optional username filter to apply when retrieving users.</param>
    public GetUsersQuery(int page, int pageSize, IEnumerable<string> group, string? username)
        : this()
    {
        this.Page = page > 0 ? page : 1;
        this.PageSize = pageSize > 0 ? pageSize : 10;
        this.Group = group;
        this.Username = username;
    }

    /// <summary>
    /// Gets or sets the page number to retrieve. Defaults to 1.
    /// </summary>
    public int Page { get; set; } = 1;

    /// <summary>
    /// Gets or sets the number of items per page. Defaults to 10.
    /// </summary>
    public int PageSize { get; set; } = 10;

    /// <summary>
    /// Gets or sets the optional group filters to apply when retrieving users.
    /// </summary>
    public IEnumerable<string> Group { get; set; } = [];

    /// <summary>
    /// Gets or sets the optional username filter to apply when retrieving users.
    /// </summary>
    public string? Username { get; set; }
}
