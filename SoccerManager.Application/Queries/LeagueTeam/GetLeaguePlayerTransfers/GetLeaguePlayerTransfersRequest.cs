using MediatR;

namespace SoccerManager.Application.Queries.LeagueTeam.GetLeaguePlayerTransfers;

/// <summary>
/// Represents a request to retrieve a player's transfers and loans in the league of a league team.
/// </summary>
public sealed record GetLeaguePlayerTransfersRequest : IRequest<GetLeaguePlayerTransfersResponse>
{
    /// <summary>
    /// The identifier of the league team the caller is managing, whose league is the one to look in.
    /// </summary>
    public int LeagueTeamId { get; init; }

    /// <summary>
    /// The identifier of the player whose transfers and loans are being requested.
    /// </summary>
    public int PlayerId { get; init; }
}
