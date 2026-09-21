using SoccerManager.Application.Dtos;

namespace SoccerManager.Application.Queries.Formation.GetFormations;

/// <summary>
/// Represents the result of a <see cref="GetFormationsRequest"/> query.
/// </summary>
public sealed record GetFormationsResponse
{
    /// <summary>
    /// The list of all formations.
    /// </summary>
    public required List<FormationDto> Data { get; init; }
}
