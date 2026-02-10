namespace Hiberus.Industria.Templates.Aspire.React.Server.Domain.Common.Interfaces;

/// <summary>
/// Provides abstraction for accessing the current system time.
/// This enables time-based behavior to be testable by allowing tests to control time.
/// </summary>
public interface ISystemClock
{
    /// <summary>
    /// Gets the current UTC time.
    /// </summary>
    DateTimeOffset UtcNow { get; }
}
