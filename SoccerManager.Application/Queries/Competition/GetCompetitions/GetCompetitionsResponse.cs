using SoccerManager.Application.Dtos;

namespace SoccerManager.Application.Queries.Competition.GetCompetitions;

/// <summary>
/// Represents the result of a <see cref="GetCompetitionsRequest"/> query.
/// </summary>
public sealed record GetCompetitionsResponse
{
    /// <summary>
    /// The list of competitions belonging to the requested league.
    /// </summary>
    public required List<CompetitionDto> Data { get; init; }
}
