using Hiberus.Industria.Templates.Aspire.React.Server.Domain.Common.Interfaces;

namespace Hiberus.Industria.Templates.Aspire.React.Server.Infrastructure.Common;

/// <summary>
/// Production implementation of ISystemClock that returns the actual system time.
/// </summary>
public class SystemClock : ISystemClock
{
    /// <summary>
    /// Gets the current UTC time from the system.
    /// </summary>
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
}
