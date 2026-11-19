namespace SoccerManager.Application.Dtos;

/// <summary>
/// Represents the loan bids of a club, placed and received: the pending ones, then the completed ones (accepted or
/// completed). Rejected bids are not listed.
/// </summary>
public sealed record LeagueTeamLoanBidsDto
{
    /// <summary>
    /// The pending loan bids, newest first.
    /// </summary>
    public required IReadOnlyList<LeagueTeamLoanBidDto> Pending { get; init; }

    /// <summary>
    /// The accepted and completed loan bids, newest first.
    /// </summary>
    public required IReadOnlyList<LeagueTeamLoanBidDto> Completed { get; init; }
}
