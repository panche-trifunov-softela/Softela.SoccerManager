CREATE OR ALTER PROCEDURE dbo.GetMatchTeamStatistics
    @MatchId INT
AS
BEGIN
    SET NOCOUNT ON;

    -- Ordered by IsHomeTeam DESC so the home team's row comes back first.
    SELECT Id, TeamId, MatchId, IsHomeTeam, ShotsTotal, ShotsOnTarget, Possession, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy
    FROM dbo.MatchTeamStatistics
    WHERE MatchId = @MatchId
    ORDER BY IsHomeTeam DESC;
END
