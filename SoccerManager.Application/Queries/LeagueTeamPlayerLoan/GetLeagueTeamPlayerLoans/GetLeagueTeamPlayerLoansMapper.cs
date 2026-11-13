using SoccerManager.Application.Dtos;

namespace SoccerManager.Application.Queries.LeagueTeamPlayerLoan.GetLeagueTeamPlayerLoans;

/// <summary>
/// Maps loan domain entities to <see cref="LeagueTeamPlayerLoanDto"/> instances.
/// </summary>
public static class GetLeagueTeamPlayerLoansMapper
{
    /// <summary>
    /// Converts a loan entity into its DTO representation.
    /// </summary>
    /// <param name="leagueTeamPlayerLoan">The loan entity to convert.</param>
    /// <returns>The corresponding <see cref="LeagueTeamPlayerLoanDto"/>.</returns>
    public static LeagueTeamPlayerLoanDto ToDto(SoccerManager.Domain.Entities.LeagueTeamPlayerLoan leagueTeamPlayerLoan)
    {
        return new LeagueTeamPlayerLoanDto
        {
            Id = leagueTeamPlayerLoan.Id,
            LeagueTeamPlayerId = leagueTeamPlayerLoan.LeagueTeamPlayerId,
            ToLeagueTeamId = leagueTeamPlayerLoan.ToLeagueTeamId,
            SeasonId = leagueTeamPlayerLoan.SeasonId,
            LoanFee = leagueTeamPlayerLoan.LoanFee,
            SeasonsLength = leagueTeamPlayerLoan.SeasonsLength,
            BuyOptionValue = leagueTeamPlayerLoan.BuyOptionValue,
            SalaryCoverageInPercent = leagueTeamPlayerLoan.SalaryCoverageInPercent,
            Status = leagueTeamPlayerLoan.Status,
            CreatedAt = leagueTeamPlayerLoan.CreatedAt,
            ModifiedAt = leagueTeamPlayerLoan.ModifiedAt,
        };
    }
}
