namespace Hiberus.Industria.Templates.Aspire.React.Server.Application.Common.Configurations;

/// <summary>
/// Represents the configuration options for the OAuth2 authentication flow.
/// </summary>
public class OAuthOptions
{
    /// <summary>
    /// Gets or sets the OAuth2 valid issuers.
    /// </summary>
    public IEnumerable<string> ValidIssuers { get; set; } = default!;
}
