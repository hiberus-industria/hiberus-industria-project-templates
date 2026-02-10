using Hiberus.Industria.Templates.Aspire.React.Server.Application.Common.Interfaces;

namespace Hiberus.Industria.Templates.Aspire.React.Server.Infrastructure.Persistence.Repositories;

/// <summary>
/// Entity Framework implementation of the Unit of Work pattern.
/// </summary>
public sealed class EfUnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext dbContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="EfUnitOfWork"/> class.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    public EfUnitOfWork(ApplicationDbContext dbContext) => this.dbContext = dbContext;

    /// <inheritdoc/>
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken) =>
        this.dbContext.SaveChangesAsync(cancellationToken);

    /// <inheritdoc/>
    public async Task<IDisposable> BeginTransactionAsync(CancellationToken cancellationToken) =>
        await this
            .dbContext.Database.BeginTransactionAsync(cancellationToken)
            .ConfigureAwait(false);
}
