-- One club's lineup rows for a match, each joined to its formation slot and to the player behind its position rating, in slot
-- order. SlotPositionId is the position the slot asks for (NULL on a reserve slot); PlayedPositionId is the position of the
-- rating the row uses. The rows can span several formations (the removed per-slot endpoints allowed it); the caller picks the lineup's.
CREATE OR ALTER PROCEDURE dbo.GetMatchLineupRows
    @MatchId      INT,
    @LeagueTeamId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT mfpp.Id,
           fp.FormationId,
           mfpp.FormationPositionId,
           fp.SlotNumber,
           fp.PositionId AS SlotPositionId,
           mfpp.PlayerPositionId,
           pp.PlayerId,
           pp.PositionId AS PlayedPositionId,
           mfpp.ConditionOnMatch,
           mfpp.QualityAtPositionOnMatch,
           mfpp.IsSuspended
    FROM dbo.MatchFormationPlayerPositions mfpp
        INNER JOIN dbo.FormationPositions fp ON fp.Id = mfpp.FormationPositionId
        INNER JOIN dbo.PlayerPositions pp ON pp.Id = mfpp.PlayerPositionId
    WHERE mfpp.MatchId = @MatchId
      AND mfpp.LeagueTeamId = @LeagueTeamId
    ORDER BY fp.SlotNumber, mfpp.Id;
END
