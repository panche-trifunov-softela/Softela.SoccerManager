using SoccerManager.Application.Dtos;

namespace SoccerManager.Application.Queries.Formation.GetFormationById;

/// <summary>
/// Represents the result of a <see cref="GetFormationByIdRequest"/> query.
/// </summary>
public sealed record GetFormationByIdResponse
{
    /// <summary>
    /// The requested formation.
    /// </summary>
    public required FormationDto Data { get; init; }
}
