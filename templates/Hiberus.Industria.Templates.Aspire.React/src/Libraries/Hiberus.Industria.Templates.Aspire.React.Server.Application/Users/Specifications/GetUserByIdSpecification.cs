using Ardalis.Specification;
using Hiberus.Industria.Templates.Aspire.React.Server.Domain.Users.Entities;

namespace Hiberus.Industria.Templates.Aspire.React.Server.Application.Users.Specifications;

/// <summary>
/// Specification to get a user by its identifier.
/// </summary>
public sealed class GetUserByIdSpecification : Specification<User>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetUserByIdSpecification"/> class.
    /// </summary>
    /// <param name="userId">The identifier of the user.</param>
    public GetUserByIdSpecification(long userId)
    {
        this.Query.Where(user => user.Id == userId);
    }
}
