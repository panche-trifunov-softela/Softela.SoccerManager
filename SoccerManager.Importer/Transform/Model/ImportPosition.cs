using SoccerManager.Domain.Enums;

namespace SoccerManager.Importer.Transform.Model;

/// <summary>
/// One of the 13 positions the importer recognizes.
/// </summary>
/// <param name="Name">The position's name, exactly as spelled in <see cref="PositionCatalog"/>.</param>
/// <param name="Area">The pitch area the position occupies.</param>
/// <param name="Side">The pitch side the position occupies.</param>
public sealed record ImportPosition(string Name, PositionArea Area, PositionSide Side);
