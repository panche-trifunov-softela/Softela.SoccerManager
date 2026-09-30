using System.Text;

namespace SoccerManager.Importer.Wikidata;

/// <summary>
/// Builds the SPARQL query that looks up a batch of players on Wikidata by their Transfermarkt player id.
/// </summary>
public static class WikidataPlayerQuery
{
    /// <summary>
    /// Builds a SPARQL query matching Wikidata items whose Transfermarkt player ID (P2446) is one of the given
    /// ids. The date of birth and citizenship are both optional, so a player with neither still comes back.
    /// </summary>
    /// <param name="transfermarktIds">The Transfermarkt player ids to look up.</param>
    /// <returns>The SPARQL query text.</returns>
    public static string Build(IReadOnlyCollection<int> transfermarktIds)
    {
        // P2446 values are strings, so each id is quoted rather than given as a plain integer literal.
        var values = string.Join(' ', transfermarktIds.Select(id => $"\"{id}\""));

        var query = new StringBuilder();
        query.AppendLine("SELECT ?item ?itemLabel ?tm ?dob ?dobPrecision ?citizenship ?citizenshipLabel WHERE {");
        query.AppendLine($"  VALUES ?tm {{ {values} }}");
        query.AppendLine("  ?item wdt:P2446 ?tm .");
        query.AppendLine("  OPTIONAL {");
        query.AppendLine("    ?item p:P569 ?st .");
        query.AppendLine("    ?st psv:P569 [ wikibase:timeValue ?dob ; wikibase:timePrecision ?dobPrecision ] .");
        query.AppendLine("    ?st wikibase:rank ?rank .");
        query.AppendLine("    FILTER(?rank != wikibase:DeprecatedRank)");
        query.AppendLine("  }");
        query.AppendLine("  OPTIONAL { ?item wdt:P27 ?citizenship . }");
        query.AppendLine("  SERVICE wikibase:label { bd:serviceParam wikibase:language \"en\". }");
        query.Append('}');

        return query.ToString();
    }
}
