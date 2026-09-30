using Microsoft.Extensions.Options;
using SoccerManager.Importer.Dataset;
using SoccerManager.Importer.Transform.Model;

namespace SoccerManager.Importer.Transform;

/// <summary>
/// Builds a player's position list from their sub-position and starting lineup history.
/// </summary>
public sealed class PlayerPositionBuilder
{
    private readonly PositionsOptions _options;

    /// <summary>
    /// Initializes a new instance of the <see cref="PlayerPositionBuilder"/> class.
    /// </summary>
    /// <param name="options">The secondary position threshold configuration.</param>
    public PlayerPositionBuilder(IOptions<PositionsOptions> options)
    {
        _options = options.Value;
    }

    /// <summary>
    /// Builds a player's positions: the main position, at quality 100, followed by any secondary ones. The main
    /// position is the player's sub-position when it is one of the 13 recognized positions, else the starting
    /// lineup position they started most, else there is no main position. A secondary position is any other lineup
    /// position started at least <see cref="PositionsOptions.SecondaryMinStarts"/> times and at least
    /// <see cref="PositionsOptions.SecondaryMinShare"/> of the player's total starts, with a quality scaled between
    /// the configured base and cap by its share of the player's busiest position.
    /// </summary>
    /// <param name="subPosition">The player's sub-position name, or <see langword="null"/> when blank.</param>
    /// <param name="positionStarts">The player's starting lineup starts by normalized position name, or <see langword="null"/> when they had none.</param>
    /// <returns>The player's positions, possibly empty when neither a sub-position nor any lineup starts are available.</returns>
    public IReadOnlyList<ImportPlayerPosition> Build(string? subPosition, PlayerPositionStarts? positionStarts)
    {
        var starts = positionStarts?.StartsByPosition ?? new Dictionary<string, int>();
        var main = ResolveMainPosition(subPosition, starts);

        var positions = new List<ImportPlayerPosition>();
        if (main is not null)
        {
            positions.Add(new ImportPlayerPosition(main, 100));
        }

        if (starts.Count == 0)
        {
            return positions;
        }

        var maxStarts = starts.Values.Max();

        if (maxStarts <= 0)
        {
            // A non-positive busiest-position count would make the share-of-busiest scaling below divide by zero
            // or by a negative span; treat it as no meaningful lineup history and keep only the main position.
            return positions;
        }

        var totalStarts = starts.Values.Sum();

        foreach (var (position, count) in starts.OrderBy(entry => entry.Key, StringComparer.Ordinal))
        {
            if (string.Equals(position, main, StringComparison.Ordinal))
            {
                continue;
            }

            if (count < _options.SecondaryMinStarts || count < _options.SecondaryMinShare * totalStarts)
            {
                continue;
            }

            var span = _options.SecondarySpan * count / maxStarts;
            var quality = Math.Min(_options.SecondaryCap, Math.Round(_options.SecondaryBase + span, MidpointRounding.AwayFromZero));

            positions.Add(new ImportPlayerPosition(position, (int)quality));
        }

        return positions;
    }

    private static string? ResolveMainPosition(string? subPosition, IReadOnlyDictionary<string, int> starts)
    {
        if (subPosition is not null && LineupPositionNames.All.Contains(subPosition))
        {
            return subPosition;
        }

        if (starts.Count == 0)
        {
            return null;
        }

        return starts
            .OrderByDescending(entry => entry.Value)
            .ThenBy(entry => entry.Key, StringComparer.Ordinal)
            .First()
            .Key;
    }
}
