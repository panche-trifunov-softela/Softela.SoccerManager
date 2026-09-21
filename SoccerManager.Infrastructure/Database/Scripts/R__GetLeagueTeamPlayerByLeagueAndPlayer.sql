-- Backs the snapshot the MatchFormationPlayerPosition handlers take of a player's registration; UQ_LeagueTeamPlayers_LeagueId_PlayerId guarantees at most one row, since a player is registered with one team per league.
CREATE OR ALTER PROCEDURE dbo.GetLeagueTeamPlayerByLeagueAndPlayer
    @LeagueId INT,
    @PlayerId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, LeagueId, TeamId, PlayerId, ContractLength, ContractSalaryPerWeek, SquadNumber, FansFavoritePlayer, Morale, TransfermarketValue, WantedStarterAppearances, WantedTotalAppearances, Condition, IsSuspendedDomesticCompetition, IsSuspendedContinentalCompetition, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy
    FROM dbo.LeagueTeamPlayers
    WHERE LeagueId = @LeagueId AND PlayerId = @PlayerId;
END
