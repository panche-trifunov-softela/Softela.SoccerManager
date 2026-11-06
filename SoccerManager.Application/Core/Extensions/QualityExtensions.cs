using SoccerManager.Domain.Enums;

namespace SoccerManager.Application.Core.Extensions;

/// <summary>
/// Reads a player's quality in a position as how well they suit it.
/// </summary>
public static class QualityExtensions
{
    private const int MainQuality = 100;

    private const int NaturalFitMinQuality = 95;

    private const int CanPlayMinQuality = 75;

    /// <summary>
    /// Converts a quality into a fit: 100 is the main position, 95 and above a natural fit, 75 and above a position the
    /// player can play, and anything lower a poor fit.
    /// </summary>
    /// <param name="quality">The player's quality in the position, from 1 to 100.</param>
    /// <returns>How well the player suits the position.</returns>
    public static PositionFit ToPositionFit(this int quality)
    {
        return quality switch
        {
            >= MainQuality => PositionFit.Main,
            >= NaturalFitMinQuality => PositionFit.NaturalFit,
            >= CanPlayMinQuality => PositionFit.CanPlay,
            _ => PositionFit.PoorFit,
        };
    }
}
