namespace SoccerManager.Application.Dtos;

/// <summary>
/// Represents the transfer bids of a club, placed and received: the pending ones, then the completed ones (accepted or
/// completed). Rejected bids are not listed.
/// </summary>
public sealed record LeagueTeamTransferBidsDto
{
    /// <summary>
    /// The pending transfer bids, newest first.
    /// </summary>
    public required IReadOnlyList<LeagueTeamTransferBidDto> Pending { get; init; }

    /// <summary>
    /// The accepted and completed transfer bids, newest first.
    /// </summary>
    public required IReadOnlyList<LeagueTeamTransferBidDto> Completed { get; init; }
}
