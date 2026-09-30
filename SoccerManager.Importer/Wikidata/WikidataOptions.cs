namespace SoccerManager.Importer.Wikidata;

/// <summary>
/// Configuration for enriching harvested players from Wikidata.
/// </summary>
public sealed class WikidataOptions
{
    /// <summary>The name of the configuration section this options class binds to.</summary>
    public const string SectionName = "Wikidata";

    /// <summary>The Wikidata SPARQL query endpoint.</summary>
    public string Endpoint { get; set; } = "https://query.wikidata.org/sparql";

    /// <summary>
    /// The User-Agent header sent with every request, as the Wikimedia User-Agent policy requires:
    /// "&lt;client&gt;/&lt;version&gt; (&lt;contact information&gt;) &lt;library&gt;/&lt;version&gt;". Defaults to
    /// this importer's own maintainer contact page on Wikidata, which the policy accepts as contact information;
    /// override with <c>--Wikidata:UserAgent=...</c> for a different operator or contact.
    /// </summary>
    public string UserAgent { get; set; } = "SoccerManagerImporter/1.0 (https://www.wikidata.org/wiki/User:Trifunov)";

    /// <summary>The number of Transfermarkt player ids queried per request.</summary>
    public int BatchSize { get; set; } = 200;

    /// <summary>The time allowed for a single request to complete.</summary>
    public int RequestTimeoutSeconds { get; set; } = 90;

    /// <summary>The maximum number of attempts for a request before it is considered failed.</summary>
    public int MaxAttempts { get; set; } = 5;
}
