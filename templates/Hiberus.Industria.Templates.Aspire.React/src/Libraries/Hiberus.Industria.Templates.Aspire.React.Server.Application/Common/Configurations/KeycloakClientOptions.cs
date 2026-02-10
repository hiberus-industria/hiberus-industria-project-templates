namespace Hiberus.Industria.Templates.Aspire.React.Server.Application.Common.Configurations;

/// <summary>
/// Defines a set of options used to perform Keycloak Admin HTTP Client calls.
/// </summary>
public class KeycloakClientOptions
{
    /// <summary>
    /// Gets or sets the authorization server URL.
    /// </summary>
    public required Uri AuthServerUrl { get; set; }

    /// <summary>
    /// Gets or sets the OAuth2 token endpoint URL.
    /// </summary>
    public required Uri TokenUrl { get; set; }

    /// <summary>
    /// Gets or sets the realm name.
    /// </summary>
    public required string Realm { get; set; } = "master";

    /// <summary>
    /// Gets or sets the destination realm name.
    /// </summary>
    public required string DestinationRealm { get; set; } = "templates-aspire-react";

    /// <summary>
    /// Gets or sets the OAuth2 client ID.
    /// </summary>
    public required string ClientId { get; set; } = "admin-api";

    /// <summary>
    /// Gets or sets the OAuth2 client secret.
    /// </summary>
    public required string ClientSecret { get; set; }

    /// <summary>
    /// Gets or sets the default user password for Keycloak.
    /// </summary>
    public required string DefaultUserPassword { get; set; } = "DefaultPassword1234!@#";
}
