-- One club's substitution lineup rows for a match, every substitution's, each joined to its formation slot and to the player
-- behind its position rating, ordered by minute, situation, then slot: the columns of dbo.GetMatchLineupRows plus the
-- substitution's minute and situation. The caller groups them by minute and situation. It never writes.
CREATE OR ALTER PROCEDURE dbo.GetMatchLineupSubstitutionRows
    @MatchId      INT,
    @LeagueTeamId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT mfpps.Id,
           mfpps.MinuteAt,
           mfpps.Situation,
           fp.FormationId,
           mfpps.FormationPositionId,
           fp.SlotNumber,
           fp.PositionId AS SlotPositionId,
           mfpps.PlayerPositionId,
           pp.PlayerId,
           pp.PositionId AS PlayedPositionId,
           mfpps.ConditionOnMatch,
           mfpps.QualityAtPositionOnMatch,
           mfpps.IsSuspended
    FROM dbo.MatchFormationPlayerPositionSubstitutions mfpps
        INNER JOIN dbo.FormationPositions fp ON fp.Id = mfpps.FormationPositionId
        INNER JOIN dbo.PlayerPositions pp ON pp.Id = mfpps.PlayerPositionId
    WHERE mfpps.MatchId = @MatchId
      AND mfpps.LeagueTeamId = @LeagueTeamId
    ORDER BY mfpps.MinuteAt, mfpps.Situation, fp.SlotNumber, mfpps.Id;
END
