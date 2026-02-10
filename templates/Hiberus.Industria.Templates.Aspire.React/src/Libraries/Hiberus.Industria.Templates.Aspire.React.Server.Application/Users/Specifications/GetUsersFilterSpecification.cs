using Ardalis.Specification;
using Hiberus.Industria.Templates.Aspire.React.Server.Domain.Users.Entities;

namespace Hiberus.Industria.Templates.Aspire.React.Server.Application.Users.Specifications;

/// <summary>
/// Base specification with common filters for users query.
/// Descendants can add projection or use as-is for counting.
/// </summary>
public class GetUsersFilterSpecification : Specification<User>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetUsersFilterSpecification"/> class.
    /// </summary>
    /// <param name="groups">Optional group filter collection.</param>
    /// <param name="username">Optional username filter (partial match).</param>
    public GetUsersFilterSpecification(IEnumerable<string> groups, string? username)
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
    }
}
