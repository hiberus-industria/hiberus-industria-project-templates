namespace Hiberus.Industria.Templates.Aspire.React.Server.Extensions;

/// <summary>
/// Provides extension methods for parsing query ranges.
/// </summary>
public static class QueryRangeParsersExtensions
{
    /// <summary>
    /// Converts an array of long values representing Unix time in milliseconds to a tuple of DateTimeOffset.
    /// </summary>
    /// <param name="arr">The array of long values.</param>
    /// <returns>A tuple containing the start and end DateTimeOffset, or null if the array is invalid.</returns>
    public static (DateTimeOffset? Start, DateTimeOffset? End)? ToDateTimeOffsetRange(
        this long[]? arr
    )
    {
        if (arr is null || arr.Length < 2)
        {
            return null;
        }

        return (
            DateTimeOffset.FromUnixTimeMilliseconds(arr[0]),
            DateTimeOffset.FromUnixTimeMilliseconds(arr[1])
        );
    }
}
