namespace SoccerManager.Application.Services;

/// <summary>
/// The fixed limits of a club's substitutions for a match.
/// </summary>
public static class MatchSubstitutionLimits
{
    /// <summary>
    /// The minutes a substitution can take effect at, earliest first. A club's substitutions for one match all share one of them.
    /// </summary>
    public static readonly IReadOnlyList<int> Minutes = [45, 60, 75];

    /// <summary>
    /// The minutes in words, for messages: <c>45, 60 or 75</c>.
    /// </summary>
    public static readonly string MinutesInWords = $"{string.Join(", ", Minutes.Take(Minutes.Count - 1))} or {Minutes[^1]}";

    /// <summary>
    /// The most players a substitution's starting eleven may hold who are not in the main starting eleven.
    /// </summary>
    public const int MaxPlayersBroughtOn = 5;
}
