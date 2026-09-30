namespace SoccerManager.Importer.Dataset;

/// <summary>
/// One club's running league totals, accumulated as the games table streams.
/// </summary>
internal sealed class ClubTotals
{
    /// <summary>The number of league games played.</summary>
    public int Games { get; private set; }

    /// <summary>The league points earned (3 for a win, 1 for a draw, 0 for a loss).</summary>
    public int Points { get; private set; }

    /// <summary>The goals scored across those games.</summary>
    public int GoalsFor { get; private set; }

    /// <summary>The goals conceded across those games.</summary>
    public int GoalsAgainst { get; private set; }

    /// <summary>
    /// Folds one game's result into the running totals.
    /// </summary>
    /// <param name="points">The league points earned for this game.</param>
    /// <param name="goalsFor">The goals scored in this game.</param>
    /// <param name="goalsAgainst">The goals conceded in this game.</param>
    public void Add(int points, int goalsFor, int goalsAgainst)
    {
        Games++;
        Points += points;
        GoalsFor += goalsFor;
        GoalsAgainst += goalsAgainst;
    }
}
