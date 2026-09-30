namespace SoccerManager.Importer.Wikidata;

/// <summary>
/// One Wikidata player's running set of distinct dates of birth and citizenships, accumulated while a SPARQL
/// result set's rows are grouped by Transfermarkt id.
/// </summary>
/// <param name="qid">The player's Wikidata entity id, e.g. "Q615".</param>
/// <param name="label">The player's Wikidata label.</param>
internal sealed class WikidataPlayerAccumulator(string qid, string label)
{
    /// <summary>The player's Wikidata entity id, e.g. "Q615".</summary>
    public string Qid { get; } = qid;

    /// <summary>The player's Wikidata label.</summary>
    public string Label { get; } = label;

    /// <summary>The distinct dates of birth recorded across the result rows for this player.</summary>
    public HashSet<WikidataDateOfBirth> DatesOfBirth { get; } = new();

    /// <summary>The distinct citizenships recorded across the result rows for this player.</summary>
    public HashSet<WikidataCitizenship> Citizenships { get; } = new();
}
