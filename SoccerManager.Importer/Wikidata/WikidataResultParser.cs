using System.Globalization;
using System.Text.Json;

namespace SoccerManager.Importer.Wikidata;

/// <summary>
/// Parses a Wikidata SPARQL JSON result set into <see cref="WikidataPlayer"/> records, grouping the rows returned
/// for each player (one row per date-of-birth or citizenship combination) into a single distinct set of each.
/// </summary>
public static class WikidataResultParser
{
    /// <summary>
    /// Parses the SPARQL results JSON returned for one batch of players.
    /// </summary>
    /// <param name="json">The raw application/sparql-results+json response body.</param>
    /// <returns>One <see cref="WikidataPlayer"/> per distinct Transfermarkt id present in the results.</returns>
    public static IReadOnlyList<WikidataPlayer> Parse(string json)
    {
        using var document = JsonDocument.Parse(json);
        var bindings = document.RootElement.GetProperty("results").GetProperty("bindings");

        var accumulators = new Dictionary<int, WikidataPlayerAccumulator>();

        foreach (var binding in bindings.EnumerateArray())
        {
            var transfermarktId = int.Parse(
                GetValue(binding, "tm") ?? throw new FormatException("A Wikidata result row is missing the ?tm binding."),
                CultureInfo.InvariantCulture);
            var itemUri = GetValue(binding, "item") ?? throw new FormatException("A Wikidata result row is missing the ?item binding.");

            if (!accumulators.TryGetValue(transfermarktId, out var accumulator))
            {
                accumulator = new WikidataPlayerAccumulator(ExtractQid(itemUri), GetValue(binding, "itemLabel") ?? ExtractQid(itemUri));
                accumulators[transfermarktId] = accumulator;
            }

            AddDateOfBirth(accumulator, binding);
            AddCitizenship(accumulator, binding);
        }

        return accumulators
            .Select(pair => new WikidataPlayer(
                pair.Key,
                pair.Value.Qid,
                pair.Value.Label,
                pair.Value.DatesOfBirth.ToArray(),
                pair.Value.Citizenships.ToArray()))
            .ToArray();
    }

    private static void AddDateOfBirth(WikidataPlayerAccumulator accumulator, JsonElement binding)
    {
        var dob = GetValue(binding, "dob");
        var dobPrecision = GetValue(binding, "dobPrecision");

        if (dob is null || dobPrecision is null)
        {
            return;
        }

        accumulator.DatesOfBirth.Add(new WikidataDateOfBirth(ToDateOnly(dob), int.Parse(dobPrecision, CultureInfo.InvariantCulture)));
    }

    private static void AddCitizenship(WikidataPlayerAccumulator accumulator, JsonElement binding)
    {
        var citizenshipUri = GetValue(binding, "citizenship");

        if (citizenshipUri is null)
        {
            return;
        }

        accumulator.Citizenships.Add(new WikidataCitizenship(ExtractQid(citizenshipUri), GetValue(binding, "citizenshipLabel") ?? ExtractQid(citizenshipUri)));
    }

    private static string? GetValue(JsonElement binding, string name)
    {
        return binding.TryGetProperty(name, out var property) && property.TryGetProperty("value", out var value)
            ? value.GetString()
            : null;
    }

    private static string ExtractQid(string entityUri)
    {
        var separatorIndex = entityUri.LastIndexOf('/');
        return separatorIndex >= 0 ? entityUri[(separatorIndex + 1)..] : entityUri;
    }

    private static string ToDateOnly(string wikidataTimeValue)
    {
        // Wikidata's timeValue is an ISO-8601 instant such as "+1990-05-02T00:00:00Z"; only the date part is kept.
        return wikidataTimeValue.TrimStart('+')[..10];
    }
}
