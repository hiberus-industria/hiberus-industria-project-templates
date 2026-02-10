using Hiberus.Industria.Templates.Aspire.React.Server.Application.Common;
using Hiberus.Industria.Templates.Aspire.React.Server.Domain.Common.Interfaces;
using Hiberus.Industria.Templates.Aspire.React.Server.Domain.Users.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Hiberus.Industria.Templates.Aspire.React.Server.Infrastructure.Persistence;

/// <summary>
/// The Entity Framework Core database context for the application.
/// </summary>
public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    private readonly IClaimsAccessor claimsAccessor;
    private readonly ISystemClock systemClock;

    /// <summary>
    /// Initializes a new instance of the <see cref="ApplicationDbContext"/> class.
    /// </summary>
    /// <param name="options">The database context options.</param>
    /// <param name="claimsAccessor">The claims accessor for auditing purposes.</param>
    /// <param name="systemClock">The system clock for retrieving the current UTC time.</param>
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        IClaimsAccessor claimsAccessor,
        ISystemClock systemClock
    )
        : base(options)
    {
        this.claimsAccessor =
            claimsAccessor ?? throw new ArgumentNullException(nameof(claimsAccessor));
        this.systemClock = systemClock ?? throw new ArgumentNullException(nameof(systemClock));
    }

    /// <summary>
    /// Gets or sets the users collection.
    /// </summary>
    public DbSet<User> Users { get; set; } = null!;

    /// <summary>
    /// Overrides the SaveChanges method to handle auditing.
    /// </summary>
    /// <returns>The number of state entries written to the database.</returns>
    public override int SaveChanges()
    {
        this.SetAuditableProperties();
        return base.SaveChanges();
    }

    /// <summary>
    /// Asynchronously overrides the SaveChangesAsync method to handle auditing.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous save operation.</returns>
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        this.SetAuditableProperties();
        return await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Configures the model relationships and constraints.
    /// </summary>
    /// <param name="modelBuilder">The model builder.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }

    /// <summary>
    /// Sets the auditing properties for entities that implement IAuditable.
    /// </summary>
    private void SetAuditableProperties()
    {
        DateTime now = this.systemClock.UtcNow.DateTime;

        foreach (EntityEntry<IAuditable> entry in this.ChangeTracker.Entries<IAuditable>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                {
                    entry.CurrentValues[nameof(IAuditable.CreatedAt)] = now;
                    entry.CurrentValues[nameof(IAuditable.CreatedBy)] =
                        this.claimsAccessor.GetCurrentUsername();
                    break;
                }

                case EntityState.Modified:
                {
                    entry.CurrentValues[nameof(IAuditable.UpdatedAt)] = now;
                    entry.CurrentValues[nameof(IAuditable.UpdatedBy)] =
                        this.claimsAccessor.GetCurrentUsername();
                    break;
                }

                case EntityState.Detached:
                case EntityState.Unchanged:
                case EntityState.Deleted:
                {
                    break;
                }
            }
        }
    }
}
