namespace SoccerManager.Importer.Dataset;

/// <summary>
/// One in-scope player's running UEFA competition totals, combined across the whole window as the appearances
/// table streams.
/// </summary>
internal sealed class UefaTotals
{
    /// <summary>The UEFA minutes played.</summary>
    public int Minutes { get; private set; }

    /// <summary>The UEFA goals scored.</summary>
    public int Goals { get; private set; }

    /// <summary>The UEFA assists recorded.</summary>
    public int Assists { get; private set; }

    /// <summary>
    /// Folds one appearance row into the running totals.
    /// </summary>
    /// <param name="minutes">The minutes played in this game.</param>
    /// <param name="goals">The goals scored in this game.</param>
    /// <param name="assists">The assists recorded in this game.</param>
    public void Add(int minutes, int goals, int assists)
    {
        Minutes += minutes;
        Goals += goals;
        Assists += assists;
    }
}
