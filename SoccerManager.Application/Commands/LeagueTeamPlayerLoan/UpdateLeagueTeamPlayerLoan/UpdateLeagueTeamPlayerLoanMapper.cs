namespace SoccerManager.Application.Commands.LeagueTeamPlayerLoan.UpdateLeagueTeamPlayerLoan;

/// <summary>
/// Applies <see cref="UpdateLeagueTeamPlayerLoanRequest"/> values onto an existing domain entity.
/// </summary>
public static class UpdateLeagueTeamPlayerLoanMapper
{
    /// <summary>
    /// Applies the request values to the given loan, preserving its creation metadata.
    /// </summary>
    /// <param name="request">The request carrying the new loan values.</param>
    /// <param name="leagueTeamPlayerLoan">The loaded loan entity to mutate.</param>
    /// <param name="now">The UTC timestamp to stamp on the modified field.</param>
    /// <param name="userId">The identifier of the user performing the update.</param>
    public static void ApplyTo(UpdateLeagueTeamPlayerLoanRequest request, SoccerManager.Domain.Entities.LeagueTeamPlayerLoan leagueTeamPlayerLoan, DateTime now, Guid userId)
    {
        leagueTeamPlayerLoan.LeagueTeamPlayerId = request.LeagueTeamPlayerId;
        leagueTeamPlayerLoan.ToLeagueTeamId = request.ToLeagueTeamId;
        leagueTeamPlayerLoan.SeasonId = request.SeasonId;
        leagueTeamPlayerLoan.LoanFee = request.LoanFee;
        leagueTeamPlayerLoan.SeasonsLength = request.SeasonsLength;
        leagueTeamPlayerLoan.BuyOptionValue = request.BuyOptionValue;
        leagueTeamPlayerLoan.SalaryCoverageInPercent = request.SalaryCoverageInPercent;
        leagueTeamPlayerLoan.Status = request.Status;
        leagueTeamPlayerLoan.ModifiedAt = now;
        leagueTeamPlayerLoan.ModifiedBy = userId;
    }
}
