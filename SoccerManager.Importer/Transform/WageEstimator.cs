using Microsoft.Extensions.Options;

namespace SoccerManager.Importer.Transform;

/// <summary>
/// Estimates a player's weekly wage from their value.
/// </summary>
public sealed class WageEstimator
{
    private readonly WageOptions _options;

    /// <summary>
    /// Initializes a new instance of the <see cref="WageEstimator"/> class.
    /// </summary>
    /// <param name="options">The wage curve configuration.</param>
    public WageEstimator(IOptions<WageOptions> options)
    {
        _options = options.Value;
    }

    /// <summary>
    /// Estimates the weekly wage for a player worth <paramref name="value"/>: <c>A * value^B / 52</c>, floored at
    /// the configured minimum. A value of zero still returns the floor.
    /// </summary>
    /// <param name="value">The player's value in EUR.</param>
    /// <returns>The estimated weekly wage in EUR, rounded to the nearest whole euro.</returns>
    public decimal Estimate(decimal value)
    {
        var raw = _options.A * Math.Pow((double)value, _options.B) / 52.0;
        var weekly = Math.Max((double)_options.WeeklyFloor, raw);

        return Math.Round((decimal)weekly, 0, MidpointRounding.AwayFromZero);
    }
}
