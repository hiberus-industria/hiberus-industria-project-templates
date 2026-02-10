using Hiberus.Industria.Templates.Aspire.React.Server.Domain.Common.Interfaces;

namespace Hiberus.Industria.Templates.Aspire.React.Server.Domain.Common.Structures;

/// <summary>
/// Base class for domain entities that can record domain events.
/// </summary>
public abstract class Entity
{
    private readonly List<IDomainEvent> domainEvents = [];

    /// <summary>
    /// Gets the domain events that have been raised by this entity.
    /// </summary>
    public IReadOnlyCollection<IDomainEvent> DomainEvents => this.domainEvents.AsReadOnly();

    /// <summary>
    /// Removes all recorded domain events.
    /// </summary>
    public void ClearDomainEvents() => this.domainEvents.Clear();

    /// <summary>
    /// Records a domain event to be published by the infrastructure.
    /// </summary>
    /// <param name="event">The domain event to record.</param>
#pragma warning disable
    protected void RaiseDomainEvent(IDomainEvent @event) => this.domainEvents.Add(@event);
#pragma warning restore
}
