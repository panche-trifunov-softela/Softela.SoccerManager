CREATE OR ALTER PROCEDURE dbo.GetLeagueTeamPlayerById
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, LeagueId, TeamId, PlayerId, ContractLength, ContractSalaryPerWeek, SquadNumber, FansFavoritePlayer, Morale, TransfermarketValue, WantedStarterAppearances, WantedTotalAppearances, Condition, IsSuspendedDomesticCompetition, IsSuspendedContinentalCompetition, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy
    FROM dbo.LeagueTeamPlayers
    WHERE Id = @Id;
END
