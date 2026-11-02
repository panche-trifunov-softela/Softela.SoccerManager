-- Every slot of one formation with its position and its place on the pitch, in slot order. A reserve slot has neither; a starter
-- slot has no place until its PitchX and PitchY are written. An unknown formation, or one with no slots, returns no rows.
CREATE OR ALTER PROCEDURE dbo.GetFormationSlots
    @FormationId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT f.Id AS FormationId,
           f.Name AS FormationName,
           fp.Id AS FormationPositionId,
           fp.SlotNumber,
           fp.PositionId,
           pos.Name AS PositionName,
           pos.ShortName AS PositionShortName,
           pos.Area AS PositionArea,
           fp.PitchX,
           fp.PitchY
    FROM dbo.Formations f
        INNER JOIN dbo.FormationPositions fp ON fp.FormationId = f.Id
        LEFT JOIN dbo.Positions pos ON pos.Id = fp.PositionId
    WHERE f.Id = @FormationId
    ORDER BY fp.SlotNumber;
END
