namespace Hiberus.Industria.Templates.Aspire.React.Server.Domain.Common.Interfaces;

/// <summary>
/// Represents a domain event that occurred within the system.
/// </summary>
public interface IDomainEvent
{
    /// <summary>
    /// Gets the UTC timestamp when the event occurred.
    /// </summary>
    DateTimeOffset OccurredOnUtc { get; }

    /// <summary>
    /// Gets an optional correlation identifier to relate this event to a broader operation or request.
    /// </summary>
    string? CorrelationId { get; }
}
