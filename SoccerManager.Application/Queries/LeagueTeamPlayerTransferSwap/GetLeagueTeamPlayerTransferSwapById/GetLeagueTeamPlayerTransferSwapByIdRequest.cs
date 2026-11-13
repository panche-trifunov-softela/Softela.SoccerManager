using MediatR;

namespace SoccerManager.Application.Queries.LeagueTeamPlayerTransferSwap.GetLeagueTeamPlayerTransferSwapById;

/// <summary>
/// Represents a request to retrieve a single swap by identifier.
/// </summary>
public sealed record GetLeagueTeamPlayerTransferSwapByIdRequest : IRequest<GetLeagueTeamPlayerTransferSwapByIdResponse>
{
    /// <summary>
    /// The identifier of the swap to retrieve.
    /// </summary>
    public int Id { get; init; }
}
