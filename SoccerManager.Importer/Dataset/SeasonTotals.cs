namespace SoccerManager.Importer.Dataset;

/// <summary>
/// One in-scope player's running domestic league totals for a single window season, accumulated as the
/// appearances table streams.
/// </summary>
internal sealed class SeasonTotals
{
    private readonly Dictionary<int, int> _minutesByClub = new();

    /// <summary>The league minutes played.</summary>
    public int Minutes { get; private set; }

    /// <summary>The league goals scored.</summary>
    public int Goals { get; private set; }

    /// <summary>The league assists recorded.</summary>
    public int Assists { get; private set; }

    /// <summary>The number of league games appeared in.</summary>
    public int Appearances { get; private set; }

    /// <summary>The player's league minutes this season, keyed by the club's Transfermarkt id.</summary>
    public IReadOnlyDictionary<int, int> MinutesByClub => _minutesByClub;

    /// <summary>
    /// Folds one appearance row into the running totals.
    /// </summary>
    /// <param name="minutes">The minutes played in this game.</param>
    /// <param name="goals">The goals scored in this game.</param>
    /// <param name="assists">The assists recorded in this game.</param>
    /// <param name="clubId">The Transfermarkt id of the club the player appeared for, or <see langword="null"/> when not recorded.</param>
    public void Add(int minutes, int goals, int assists, int? clubId)
    {
        Minutes += minutes;
        Goals += goals;
        Assists += assists;
        Appearances++;

        if (clubId is { } club)
        {
            _minutesByClub.TryGetValue(club, out var current);
            _minutesByClub[club] = current + minutes;
        }
    }

    /// <summary>
    /// The Transfermarkt id of the club the player played the most league minutes for this season.
    /// </summary>
    /// <returns>The club id, or <see langword="null"/> when the player recorded no league minutes for any club this season.</returns>
    public int? MainClub()
    {
        return _minutesByClub.Count == 0
            ? null
            : _minutesByClub.OrderByDescending(pair => pair.Value).ThenBy(pair => pair.Key).First().Key;
    }
}
