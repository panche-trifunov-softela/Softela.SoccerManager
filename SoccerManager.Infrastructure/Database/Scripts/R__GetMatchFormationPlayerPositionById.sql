CREATE OR ALTER PROCEDURE dbo.GetMatchFormationPlayerPositionById
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, MatchId, TeamId, FormationPositionId, PlayerPositionId, ConditionOnMatch, QualityAtPositionOnMatch, IsSuspended, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy
    FROM dbo.MatchFormationPlayerPositions
    WHERE Id = @Id;
END
