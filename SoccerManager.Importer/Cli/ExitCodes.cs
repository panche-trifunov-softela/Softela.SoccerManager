namespace SoccerManager.Importer.Cli;

/// <summary>
/// The process exit codes the importer can return.
/// </summary>
public static class ExitCodes
{
    /// <summary>The command completed successfully.</summary>
    public const int Success = 0;

    /// <summary>The command ran but did not complete successfully.</summary>
    public const int Failure = 1;

    /// <summary>The command line or configuration was invalid.</summary>
    public const int Usage = 2;
}
