using Microsoft.Extensions.Options;
using SoccerManager.Domain.Enums;

namespace SoccerManager.Importer.Transform;

/// <summary>
/// Classifies a club's overall financial standing from its squad value and net transfer record.
/// </summary>
public sealed class FinancialStateCalculator
{
    private readonly FinancialStateOptions _options;

    /// <summary>
    /// Initializes a new instance of the <see cref="FinancialStateCalculator"/> class.
    /// </summary>
    /// <param name="options">The financial state threshold configuration.</param>
    public FinancialStateCalculator(IOptions<FinancialStateOptions> options)
    {
        _options = options.Value;
    }

    /// <summary>
    /// Classifies a club from its squad value and parsed net transfer record: the squad value places it on the
    /// VeryPoor..VeryRich scale, then a net spend at or beyond the configured step-up moves it one level richer,
    /// capped at VeryRich.
    /// </summary>
    /// <param name="squadValue">The sum of the values of the club's imported players, in EUR.</param>
    /// <param name="netTransferRecord">The club's parsed net transfer record in EUR, or <see langword="null"/> when it could not be parsed.</param>
    /// <returns>The club's financial state.</returns>
    public FinancialState Calculate(decimal squadValue, decimal? netTransferRecord)
    {
        var level = squadValue switch
        {
            _ when squadValue >= _options.VeryRichMin => FinancialState.VeryRich,
            _ when squadValue >= _options.RichMin => FinancialState.Rich,
            _ when squadValue >= _options.AverageMin => FinancialState.Average,
            _ when squadValue >= _options.PoorMin => FinancialState.Poor,
            _ => FinancialState.VeryPoor,
        };

        var isNetSpender = netTransferRecord is { } record && record <= -_options.NetSpendStepUp;
        if (isNetSpender && level < FinancialState.VeryRich)
        {
            level = (FinancialState)((byte)level + 1);
        }

        return level;
    }
}
