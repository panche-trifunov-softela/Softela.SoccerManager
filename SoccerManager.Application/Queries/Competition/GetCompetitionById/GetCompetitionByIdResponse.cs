using SoccerManager.Application.Dtos;

namespace SoccerManager.Application.Queries.Competition.GetCompetitionById;

/// <summary>
/// Represents the result of a <see cref="GetCompetitionByIdRequest"/> query.
/// </summary>
public sealed record GetCompetitionByIdResponse
{
    /// <summary>
    /// The requested competition.
    /// </summary>
    public required CompetitionDto Data { get; init; }
}
