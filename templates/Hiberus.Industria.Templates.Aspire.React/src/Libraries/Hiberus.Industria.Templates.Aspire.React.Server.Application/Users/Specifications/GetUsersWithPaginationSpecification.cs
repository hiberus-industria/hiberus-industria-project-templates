using Ardalis.Specification;
using Hiberus.Industria.Templates.Aspire.React.Server.Domain.Users.Entities;

namespace Hiberus.Industria.Templates.Aspire.React.Server.Application.Users.Specifications;

/// <summary>
/// Specification to get users with filtering and pagination.
/// </summary>
public sealed class GetUsersWithPaginationSpecification : Specification<User>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetUsersWithPaginationSpecification"/> class.
    /// </summary>
    /// <param name="page">Requested page (1-based).</param>
    /// <param name="pageSize">Page size.</param>
    /// <param name="groups">Optional group filter collection.</param>
    /// <param name="username">Optional username filter (partial match).</param>
    public GetUsersWithPaginationSpecification(
        int page,
        int pageSize,
        IEnumerable<string> groups,
        string? username
    )
    {
        this.Query.AsNoTracking();

        if (groups?.Any() == true)
        {
            this.Query.Where(user => groups.Contains(user.Group));
        }

        if (!string.IsNullOrWhiteSpace(username))
        {
            this.Query.Where(user => user.Username.Contains(username));
        }

        // Ordering
        this.Query.OrderByDescending(user => user.Id);

        // Pagination
        if (page > 0 && pageSize > 0)
        {
            int skip = (page - 1) * pageSize;
            this.Query.Skip(skip).Take(pageSize);
        }
    }
}
