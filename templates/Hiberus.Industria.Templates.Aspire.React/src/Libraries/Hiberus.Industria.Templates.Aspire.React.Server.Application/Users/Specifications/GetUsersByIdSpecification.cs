using Ardalis.Specification;
using Hiberus.Industria.Templates.Aspire.React.Server.Domain.Users.Entities;

namespace Hiberus.Industria.Templates.Aspire.React.Server.Application.Users.Specifications;

/// <summary>
/// Specification to get users by their IDs.
/// </summary>
public sealed class GetUsersByIdSpecification : Specification<User>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetUsersByIdSpecification"/> class.
    /// </summary>
    /// <param name="ids">The unique identifiers of the users.</param>
    public GetUsersByIdSpecification(IEnumerable<long> ids)
    {
        this.Query.AsNoTracking().Where(user => ids.Contains(user.Id));
    }
}
