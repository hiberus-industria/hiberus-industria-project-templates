namespace Hiberus.Industria.Templates.Aspire.React.Server.Application.Identity;

/// <summary>
/// Policy names used for authorization.
/// </summary>
public static class PolicyNames
{
    /// <summary>
    /// Policy requiring the user to have the "operator" role.
    /// </summary>
    public const string HasOperatorRole = "HasOperatorRole";

    /// <summary>
    /// Policy requiring the user to have the "administrator" role.
    /// </summary>
    public const string HasAdministratorRole = "HasAdministratorRole";

    /// <summary>
    /// Policy requiring the user to have either the "operator" or "administrator" role.
    /// </summary>
    public const string HasOperatorOrAdminRole = "HasOperatorOrAdminRole";
}
