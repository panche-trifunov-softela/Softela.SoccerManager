using MediatR;

namespace SoccerManager.Application.Queries.LeagueTeamPlayerInjury.GetLeagueTeamPlayerInjuries;

/// <summary>
/// Represents a request to retrieve every injury of a squad row.
/// </summary>
public sealed record GetLeagueTeamPlayerInjuriesRequest : IRequest<GetLeagueTeamPlayerInjuriesResponse>
{
    /// <summary>
    /// The identifier of the squad row whose injuries are being requested.
    /// </summary>
    public int LeagueTeamPlayerId { get; init; }
}
