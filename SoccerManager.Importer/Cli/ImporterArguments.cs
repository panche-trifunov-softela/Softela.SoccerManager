namespace SoccerManager.Importer.Cli;

/// <summary>
/// The parsed command line invocation of the importer.
/// </summary>
/// <param name="Verb">The command to run, e.g. "import", or <see langword="null"/> when none was given.</param>
/// <param name="DryRun">Whether the run should skip database writes and print the review report instead.</param>
/// <param name="SkipWikidata">Whether the Wikidata enrichment step should be skipped.</param>
/// <param name="ConfigurationArgs">
/// The remaining arguments, with the verb, <c>--dry-run</c> and <c>--skip-wikidata</c> removed, passed through to
/// the host's configuration.
/// </param>
public sealed record ImporterArguments(string? Verb, bool DryRun, bool SkipWikidata, string[] ConfigurationArgs);
