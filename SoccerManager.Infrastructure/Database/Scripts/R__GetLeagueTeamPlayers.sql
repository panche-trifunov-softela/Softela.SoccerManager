CREATE OR ALTER PROCEDURE dbo.GetLeagueTeamPlayers
    @LeagueTeamId INT
AS
BEGIN
    SET NOCOUNT ON;

    -- Ordered by SquadNumber so the squad comes back in shirt order.
    SELECT Id, LeagueTeamId, PlayerId, ContractLength, ContractSalaryPerWeek, SquadNumber, FansFavoritePlayer, Morale, TransfermarketValue, WantedStarterAppearances, WantedTotalAppearances, Condition, IsSuspendedDomesticCompetition, IsSuspendedContinentalCompetition, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy
    FROM dbo.LeagueTeamPlayers
    WHERE LeagueTeamId = @LeagueTeamId
    ORDER BY SquadNumber;
END
