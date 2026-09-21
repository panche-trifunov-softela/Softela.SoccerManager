using SoccerManager.Application.Dtos;

namespace SoccerManager.Application.Queries.FormationPosition.GetFormationPositionById;

/// <summary>
/// Represents the result of a <see cref="GetFormationPositionByIdRequest"/> query.
/// </summary>
public sealed record GetFormationPositionByIdResponse
{
    /// <summary>
    /// The requested formation position slot.
    /// </summary>
    public required FormationPositionDto Data { get; init; }
}
