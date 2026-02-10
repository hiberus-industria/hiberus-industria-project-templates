namespace Hiberus.Industria.Templates.Aspire.React.Server.Domain.Common.Interfaces;

/// <summary>
/// Provides access to user claims information.
/// </summary>
public interface IClaimsAccessor
{
    /// <summary>
    /// Gets the user identifier of the current user.
    /// Priority: 'sub' (Keycloak) or NameIdentifier.
    /// If no HTTP context is available, a default value is returned.
    /// </summary>
    /// <returns>The current user's identifier.</returns>
    string GetCurrentUserId();

    /// <summary>
    /// Gets the identifier of the current username from the HTTP context.
    /// </summary>
    /// <returns>The current user's identifier or 'System' if not available.</returns>
    string GetCurrentUsername();
}
