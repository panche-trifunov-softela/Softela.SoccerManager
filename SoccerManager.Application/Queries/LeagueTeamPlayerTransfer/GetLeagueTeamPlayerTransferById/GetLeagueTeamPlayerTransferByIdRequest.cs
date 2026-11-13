using MediatR;

namespace SoccerManager.Application.Queries.LeagueTeamPlayerTransfer.GetLeagueTeamPlayerTransferById;

/// <summary>
/// Represents a request to retrieve a single transfer by identifier.
/// </summary>
public sealed record GetLeagueTeamPlayerTransferByIdRequest : IRequest<GetLeagueTeamPlayerTransferByIdResponse>
{
    /// <summary>
    /// The identifier of the transfer to retrieve.
    /// </summary>
    public int Id { get; init; }
}
