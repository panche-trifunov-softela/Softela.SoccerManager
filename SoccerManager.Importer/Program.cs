using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SoccerManager.Importer;
using SoccerManager.Importer.Cli;
using SoccerManager.Importer.Import;

var arguments = ImporterArgumentsParser.Parse(args);

if (arguments.Verb is null)
{
    Console.Error.WriteLine("Usage: SoccerManager.Importer <verb> [options]");
    Console.Error.WriteLine("Verbs: import [--dry-run] [--skip-wikidata] (writing needs ConnectionStrings:soccermanager)");
    return ExitCodes.Usage;
}

if (!string.Equals(arguments.Verb, "import", StringComparison.Ordinal))
{
    Console.Error.WriteLine($"Unknown verb '{arguments.Verb}'. Verbs: import [--dry-run] [--skip-wikidata] (writing needs ConnectionStrings:soccermanager)");
    return ExitCodes.Usage;
}

var builder = Host.CreateApplicationBuilder(arguments.ConfigurationArgs);
builder.Services.AddImporterServices(builder.Configuration);

using var host = builder.Build();

using var cancellationTokenSource = new CancellationTokenSource();
Console.CancelKeyPress += (_, eventArgs) =>
{
    // Cancel our own token instead of letting the runtime kill the process outright, so an in-flight table read
    // can unwind cleanly instead of leaving a corrupt partial read behind.
    eventArgs.Cancel = true;
    cancellationTokenSource.Cancel();
};

try
{
    var importCommand = host.Services.GetRequiredService<ImportCommand>();

    return await importCommand.RunAsync(arguments.DryRun, arguments.SkipWikidata, cancellationTokenSource.Token).ConfigureAwait(false);
}
catch (OperationCanceledException)
{
    host.Services.GetRequiredService<ILogger<Program>>().LogWarning("Import cancelled.");

    return ExitCodes.Failure;
}
