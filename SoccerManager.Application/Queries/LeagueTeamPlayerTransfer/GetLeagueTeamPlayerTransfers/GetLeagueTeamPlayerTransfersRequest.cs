using MediatR;

namespace SoccerManager.Application.Queries.LeagueTeamPlayerTransfer.GetLeagueTeamPlayerTransfers;

/// <summary>
/// Represents a request to retrieve every transfer of a squad row.
/// </summary>
public sealed record GetLeagueTeamPlayerTransfersRequest : IRequest<GetLeagueTeamPlayerTransfersResponse>
{
    /// <summary>
    /// The identifier of the squad row whose transfers are being requested.
    /// </summary>
    public int LeagueTeamPlayerId { get; init; }
}
