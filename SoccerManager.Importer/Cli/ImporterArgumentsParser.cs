namespace SoccerManager.Importer.Cli;

/// <summary>
/// Parses the importer's process command line.
/// </summary>
public static class ImporterArgumentsParser
{
    private const string DryRunFlag = "--dry-run";
    private const string SkipWikidataFlag = "--skip-wikidata";

    /// <summary>
    /// Parses the raw command line arguments into a verb, the importer's own flags, and the remaining arguments
    /// meant for the host's configuration.
    /// </summary>
    /// <param name="args">The arguments passed to the process.</param>
    /// <returns>The parsed <see cref="ImporterArguments"/>.</returns>
    public static ImporterArguments Parse(string[] args)
    {
        var verb = args.Length > 0 && !args[0].StartsWith("--", StringComparison.Ordinal)
            ? args[0]
            : null;

        var startIndex = verb is not null ? 1 : 0;
        var dryRun = false;
        var skipWikidata = false;
        var configurationArgs = new List<string>(args.Length);

        for (var i = startIndex; i < args.Length; i++)
        {
            if (string.Equals(args[i], DryRunFlag, StringComparison.Ordinal))
            {
                dryRun = true;
                continue;
            }

            if (string.Equals(args[i], SkipWikidataFlag, StringComparison.Ordinal))
            {
                skipWikidata = true;
                continue;
            }

            configurationArgs.Add(args[i]);
        }

        return new ImporterArguments(verb, dryRun, skipWikidata, configurationArgs.ToArray());
    }
}
