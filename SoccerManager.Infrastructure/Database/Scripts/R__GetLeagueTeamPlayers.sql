CREATE OR ALTER PROCEDURE dbo.GetLeagueTeamPlayers
    @LeagueId INT,
    @TeamId   INT
AS
BEGIN
    SET NOCOUNT ON;

    -- Ordered by SquadNumber so the squad comes back in shirt order.
    SELECT Id, LeagueId, TeamId, PlayerId, ContractLength, ContractSalaryPerWeek, SquadNumber, FansFavoritePlayer, Morale, TransfermarketValue, WantedStarterAppearances, WantedTotalAppearances, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy
    FROM dbo.LeagueTeamPlayers
    WHERE LeagueId = @LeagueId AND TeamId = @TeamId
    ORDER BY SquadNumber;
END
