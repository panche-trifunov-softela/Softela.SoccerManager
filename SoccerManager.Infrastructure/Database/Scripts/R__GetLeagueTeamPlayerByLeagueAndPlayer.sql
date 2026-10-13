-- Backs the snapshot the MatchFormationPlayerPosition handlers take of a player's registration and the one-club-per-league check in the squad validators.
-- No database constraint guarantees at most one row any more (a race between two squad writes can let a duplicate through), so the rows come back ordered by Id and the repository's QueryFirstOrDefault returns the oldest registration (the lowest Id) when a race has left duplicates.
CREATE OR ALTER PROCEDURE dbo.GetLeagueTeamPlayerByLeagueAndPlayer
    @LeagueId INT,
    @PlayerId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT ltp.Id, ltp.LeagueTeamId, ltp.PlayerId, ltp.ContractLength, ltp.ContractSalaryPerWeek, ltp.SquadNumber, ltp.FansFavoritePlayer, ltp.Morale, ltp.TransfermarketValue, ltp.WantedStarterAppearances, ltp.WantedTotalAppearances, ltp.Condition, ltp.IsSuspendedDomesticCompetition, ltp.IsSuspendedContinentalCompetition, ltp.CreatedAt, ltp.ModifiedAt, ltp.CreatedBy, ltp.ModifiedBy
    FROM dbo.LeagueTeamPlayers ltp
        INNER JOIN dbo.LeagueTeams lt ON lt.Id = ltp.LeagueTeamId
    WHERE lt.LeagueId = @LeagueId AND ltp.PlayerId = @PlayerId
    ORDER BY ltp.Id;
END
