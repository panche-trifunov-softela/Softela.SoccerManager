CREATE OR ALTER PROCEDURE dbo.GetMatchFormationPlayerPositions
    @MatchId INT
AS
BEGIN
    SET NOCOUNT ON;

    -- Ordered by LeagueTeamId so each side's lineup comes back together, then by FormationPositionId for a stable order within it.
    SELECT Id, MatchId, LeagueTeamId, FormationPositionId, PlayerPositionId, ConditionOnMatch, QualityAtPositionOnMatch, IsSuspended, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy
    FROM dbo.MatchFormationPlayerPositions
    WHERE MatchId = @MatchId
    ORDER BY LeagueTeamId, FormationPositionId;
END
