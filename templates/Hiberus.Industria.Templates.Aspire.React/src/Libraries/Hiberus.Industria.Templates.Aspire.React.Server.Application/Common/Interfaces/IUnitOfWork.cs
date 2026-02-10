namespace Hiberus.Industria.Templates.Aspire.React.Server.Application.Common.Interfaces;

/// <summary>
/// Interface for Unit of Work pattern to manage transactions.
/// </summary>
public interface IUnitOfWork
{
    /// <summary>
    /// Saves all changes made in the context to the database.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>The number of state entries written to the database.</returns>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Begins a new transaction.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>An <see cref="IDisposable"/> representing the transaction.</returns>
    Task<IDisposable> BeginTransactionAsync(CancellationToken cancellationToken);
}
