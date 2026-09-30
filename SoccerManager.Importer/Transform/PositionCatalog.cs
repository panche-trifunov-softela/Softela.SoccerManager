using SoccerManager.Domain.Enums;
using SoccerManager.Importer.Transform.Model;

namespace SoccerManager.Importer.Transform;

/// <summary>
/// The fixed catalog of the 13 positions the importer recognizes, each with the pitch area and side it occupies.
/// The names are spelled identically to <see cref="Dataset.LineupPositionNames.All"/>, since both describe the same
/// fixed set of positions and a lookup between them relies on the strings matching exactly.
/// </summary>
public static class PositionCatalog
{
    /// <summary>The 13 positions, in a fixed, stable order.</summary>
    public static IReadOnlyList<ImportPosition> Positions { get; } = new[]
    {
        new ImportPosition("Goalkeeper", PositionArea.Goalkeeper, PositionSide.Center),
        new ImportPosition("Centre-Back", PositionArea.Defence, PositionSide.Center),
        new ImportPosition("Left-Back", PositionArea.Defence, PositionSide.Left),
        new ImportPosition("Right-Back", PositionArea.Defence, PositionSide.Right),
        new ImportPosition("Defensive Midfield", PositionArea.Midfield, PositionSide.Center),
        new ImportPosition("Central Midfield", PositionArea.Midfield, PositionSide.Center),
        new ImportPosition("Attacking Midfield", PositionArea.Midfield, PositionSide.Center),
        new ImportPosition("Left Midfield", PositionArea.Midfield, PositionSide.Left),
        new ImportPosition("Right Midfield", PositionArea.Midfield, PositionSide.Right),
        new ImportPosition("Left Winger", PositionArea.Attack, PositionSide.Left),
        new ImportPosition("Right Winger", PositionArea.Attack, PositionSide.Right),
        new ImportPosition("Second Striker", PositionArea.Attack, PositionSide.Center),
        new ImportPosition("Centre-Forward", PositionArea.Attack, PositionSide.Center),
    };

    /// <summary>Looks up a position's area by name.</summary>
    /// <param name="name">The position name, exactly as spelled in <see cref="Positions"/>.</param>
    /// <returns>The position's area, or <see langword="null"/> when <paramref name="name"/> is not one of the 13.</returns>
    public static PositionArea? AreaOf(string name)
    {
        foreach (var position in Positions)
        {
            if (string.Equals(position.Name, name, StringComparison.Ordinal))
            {
                return position.Area;
            }
        }

        return null;
    }
}
