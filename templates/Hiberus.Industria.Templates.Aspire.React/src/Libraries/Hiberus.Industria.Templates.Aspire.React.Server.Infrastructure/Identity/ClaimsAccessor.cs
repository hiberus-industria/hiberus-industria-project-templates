using System.Security.Claims;
using Hiberus.Industria.Templates.Aspire.React.Server.Domain.Common.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Hiberus.Industria.Templates.Aspire.React.Server.Infrastructure.Identity;

/// <summary>
/// Provides access to user claims from the HTTP context.
/// </summary>
public class ClaimsAccessor : IClaimsAccessor
{
    private readonly IHttpContextAccessor httpContextAccessor;

    /// <summary>
    /// Initializes a new instance of the <see cref="ClaimsAccessor"/> class.
    /// </summary>
    /// <param name="httpContextAccessor">The HTTP context accessor to retrieve the current HTTP context.</param>
    public ClaimsAccessor(IHttpContextAccessor httpContextAccessor)
    {
        this.httpContextAccessor =
            httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
    }

    /// <inheritdoc />
    public string GetCurrentUserId()
    {
        ClaimsPrincipal? user = this.httpContextAccessor.HttpContext?.User;
        if (user?.Identity?.IsAuthenticated != true)
        {
            return string.Empty; // Return empty string if no user is authenticated
        }

        return user.FindFirst("sub")?.Value
            ?? user.FindFirst(ClaimTypes.NameIdentifier)?.Value
            ?? string.Empty;
    }

    /// <inheritdoc />
    public string GetCurrentUsername()
    {
        ClaimsPrincipal? user = this.httpContextAccessor.HttpContext?.User;
        if (user?.Identity?.IsAuthenticated != true)
        {
            // Default value when no user is authenticated
            return "System";
        }

        return user.FindFirst("preferred_username")?.Value ?? "System";
    }
}
