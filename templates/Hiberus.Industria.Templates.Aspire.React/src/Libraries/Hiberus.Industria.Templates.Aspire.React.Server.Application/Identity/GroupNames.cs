namespace Hiberus.Industria.Templates.Aspire.React.Server.Application.Identity;

/// <summary>
/// Contains group names used in the application.
/// </summary>
public static class GroupNames
{
    /// <summary>
    /// The Administrators group name.
    /// </summary>
    public const string Administrators = "administrators";

    /// <summary>
    /// The Operators group name.
    /// </summary>
    public const string Operators = "operators";

    /// <summary>
    /// All group names.
    /// </summary>
    public static readonly string[] All = [Administrators, Operators];

    /// <summary>
    /// Checks if the given group is a valid group name.
    /// </summary>
    /// <param name="group">The group name to check.</param>
    /// <returns>True if the group name is valid; otherwise, false.</returns>
    public static bool IsValidGroup(string group) => All.Contains(group, StringComparer.Ordinal);
}
