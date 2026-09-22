using MediatR;

namespace SoccerManager.Application.Queries.Competition.GetCompetitions;

/// <summary>
/// Represents a request to retrieve every competition belonging to a league.
/// </summary>
public sealed record GetCompetitionsRequest : IRequest<GetCompetitionsResponse>
{
    /// <summary>
    /// The identifier of the league whose competitions are being requested.
    /// </summary>
    public int LeagueId { get; init; }
}
