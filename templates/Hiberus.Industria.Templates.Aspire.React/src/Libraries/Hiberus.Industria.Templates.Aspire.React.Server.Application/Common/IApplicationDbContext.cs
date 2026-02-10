namespace Hiberus.Industria.Templates.Aspire.React.Server.Application.Common;

/// <summary>
/// Abstraction for the application's database context.
/// This enables unit testing by allowing tests to use in-memory or mock implementations.
/// </summary>
public interface IApplicationDbContext
{
    /// <summary>
    /// Asynchronously saves all changes made in the context to the database.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>The number of state entries written to the database.</returns>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
