using Ardalis.Specification;
using Hiberus.Industria.Templates.Aspire.React.Server.Domain.Users.Entities;

namespace Hiberus.Industria.Templates.Aspire.React.Server.Application.Users.Specifications;

/// <summary>
/// Specification to get a user by its username.
/// </summary>
public sealed class GetUserByUsernameSpecification : Specification<User>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetUserByUsernameSpecification"/> class.
    /// </summary>
    /// <param name="username">The username of the user.</param>
    public GetUserByUsernameSpecification(string username)
    {
        this.Query.Where(user => user.Username == username);
    }
}
