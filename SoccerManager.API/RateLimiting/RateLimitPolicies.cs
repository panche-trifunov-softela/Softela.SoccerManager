namespace SoccerManager.API.RateLimiting;

/// <summary>
/// Names the rate-limiting policies the API registers and the per-minute limits it enforces.
/// </summary>
public static class RateLimitPolicies
{
    /// <summary>
    /// The policy for player search and league search, the endpoints an eager user calls most often. Both count against one
    /// per-caller budget.
    /// </summary>
    public const string Search = "Search";

    /// <summary>
    /// The policy for reads that return a whole table.
    /// </summary>
    public const string FullTable = "FullTable";

    /// <summary>
    /// Requests a signed-in caller may make per minute, across every endpoint.
    /// </summary>
    public const int SignedInPerMinute = 120;

    /// <summary>
    /// Requests a caller without a valid token may make per minute, counted per client IP.
    /// </summary>
    public const int AnonymousPerMinute = 10;

    /// <summary>
    /// Searches a caller may make per minute, player searches and league searches together.
    /// </summary>
    public const int SearchPerMinute = 60;

    /// <summary>
    /// Whole-table reads a caller may make per minute, shared across every endpoint under <see cref="FullTable"/>.
    /// </summary>
    public const int FullTablePerMinute = 10;
}
