using SoccerManager.Domain.Enums;

namespace SoccerManager.Application.Core.Extensions;

/// <summary>
/// Works out a player's quality in a position they have no rating for, from the position's area.
/// </summary>
public static class PositionAreaExtensions
{
    private const int SameAreaQuality = 80;

    private const int OtherAreaQuality = 60;

    /// <summary>
    /// Gives the quality of a player in a position they have no rating for: higher when the position is in the same area as
    /// their main position. Goalkeeper counts as an area like any other.
    /// </summary>
    /// <param name="area">The area of the position.</param>
    /// <param name="mainArea">The area of the player's main position.</param>
    /// <returns>The player's quality in the position.</returns>
    public static int ToUnratedQuality(this PositionArea area, PositionArea mainArea)
    {
        return area == mainArea ? SameAreaQuality : OtherAreaQuality;
    }
}
