using Ardalis.Specification.EntityFrameworkCore;
using Hiberus.Industria.Templates.Aspire.React.Server.Application.Common.Interfaces;

namespace Hiberus.Industria.Templates.Aspire.React.Server.Infrastructure.Persistence.Repositories;

/// <summary>
/// Entity Framework implementation of the repository pattern.
/// </summary>
/// <typeparam name="T">The type of the entity.</typeparam>
public sealed class EfRepository<T> : RepositoryBase<T>, IReadRepository<T>, IRepository<T>
    where T : class
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EfRepository{T}"/> class.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    public EfRepository(ApplicationDbContext dbContext)
        : base(dbContext) { }
}
