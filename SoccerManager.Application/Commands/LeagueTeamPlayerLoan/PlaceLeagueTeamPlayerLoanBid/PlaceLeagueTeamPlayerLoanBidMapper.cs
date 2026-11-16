using SoccerManager.Application.Services;
using SoccerManager.Domain.Enums;

namespace SoccerManager.Application.Commands.LeagueTeamPlayerLoan.PlaceLeagueTeamPlayerLoanBid;

/// <summary>
/// Maps <see cref="PlaceLeagueTeamPlayerLoanBidRequest"/> instances to domain entities.
/// </summary>
public static class PlaceLeagueTeamPlayerLoanBidMapper
{
    /// <summary>
    /// The share of the player's wage a bid's loan covers: the player keeps the same salary at the bidding club, so the
    /// loan covers all of it.
    /// </summary>
    private const int SalaryCoverageInPercent = 100;

    /// <summary>
    /// Creates a new pending loan entity from the given request. The squad row and the season come from the resolved
    /// target, never from the caller.
    /// </summary>
    /// <param name="request">The request carrying the bid values.</param>
    /// <param name="target">The resolved target of the bid.</param>
    /// <param name="now">The UTC timestamp to stamp on the created and modified fields.</param>
    /// <param name="userId">The identifier of the user placing the bid.</param>
    /// <returns>A new, unsaved loan entity.</returns>
    public static SoccerManager.Domain.Entities.LeagueTeamPlayerLoan ToDomainEntity(PlaceLeagueTeamPlayerLoanBidRequest request, BidTarget target, DateTime now, Guid userId)
    {
        return new SoccerManager.Domain.Entities.LeagueTeamPlayerLoan
        {
            LeagueTeamPlayerId = target.LeagueTeamPlayerId,
            ToLeagueTeamId = request.LeagueTeamId,
            SeasonId = target.SeasonId,
            LoanFee = request.LoanFee,
            SeasonsLength = request.SeasonsLength,
            BuyOptionValue = request.BuyOptionValue,
            SalaryCoverageInPercent = SalaryCoverageInPercent,
            Status = TransferStatus.Pending,
            CreatedAt = now,
            CreatedBy = userId,
            ModifiedAt = now,
            ModifiedBy = userId,
        };
    }
}
