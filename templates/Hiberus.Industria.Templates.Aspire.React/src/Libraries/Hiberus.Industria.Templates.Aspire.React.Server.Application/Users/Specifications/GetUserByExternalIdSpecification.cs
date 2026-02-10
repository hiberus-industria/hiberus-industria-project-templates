using Ardalis.Specification;
using Hiberus.Industria.Templates.Aspire.React.Server.Domain.Users.Entities;

namespace Hiberus.Industria.Templates.Aspire.React.Server.Application.Users.Specifications;

/// <summary>
/// Specification to get a user by their external identifier.
/// </summary>
public sealed class GetUserByExternalIdSpecification : Specification<User>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetUserByExternalIdSpecification"/> class.
    /// </summary>
    /// <param name="externalId">The external identifier of the user.</param>
    public GetUserByExternalIdSpecification(Guid externalId)
    {
        this.Query.Where(user => user.ExternalId == externalId);
    }
}
