using MediatR;

namespace SoccerManager.Application.Queries.LeagueTeamPlayer.GetLeagueTeamPlayerById;

/// <summary>
/// Represents a request to retrieve a single league team player by identifier.
/// </summary>
public sealed record GetLeagueTeamPlayerByIdRequest : IRequest<GetLeagueTeamPlayerByIdResponse>
{
    /// <summary>
    /// The identifier of the league team player to retrieve.
    /// </summary>
    public int Id { get; init; }
}
