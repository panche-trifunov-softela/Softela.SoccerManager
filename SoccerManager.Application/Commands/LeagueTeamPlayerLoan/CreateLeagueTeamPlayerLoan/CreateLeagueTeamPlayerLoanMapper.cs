namespace SoccerManager.Application.Commands.LeagueTeamPlayerLoan.CreateLeagueTeamPlayerLoan;

/// <summary>
/// Maps <see cref="CreateLeagueTeamPlayerLoanRequest"/> instances to domain entities.
/// </summary>
public static class CreateLeagueTeamPlayerLoanMapper
{
    /// <summary>
    /// Creates a new loan entity from the given request.
    /// </summary>
    /// <param name="request">The request carrying the loan values.</param>
    /// <param name="now">The UTC timestamp to stamp on the created and modified fields.</param>
    /// <param name="userId">The identifier of the user creating the loan.</param>
    /// <returns>A new, unsaved loan entity.</returns>
    public static SoccerManager.Domain.Entities.LeagueTeamPlayerLoan ToDomainEntity(CreateLeagueTeamPlayerLoanRequest request, DateTime now, Guid userId)
    {
        return new SoccerManager.Domain.Entities.LeagueTeamPlayerLoan
        {
            LeagueTeamPlayerId = request.LeagueTeamPlayerId,
            ToLeagueTeamId = request.ToLeagueTeamId,
            SeasonId = request.SeasonId,
            LoanFee = request.LoanFee,
            SeasonsLength = request.SeasonsLength,
            BuyOptionValue = request.BuyOptionValue,
            SalaryCoverageInPercent = request.SalaryCoverageInPercent,
            Status = request.Status,
            CreatedAt = now,
            CreatedBy = userId,
            ModifiedAt = now,
            ModifiedBy = userId,
        };
    }
}
