CREATE OR ALTER PROCEDURE dbo.InsertMatchFormationPlayerPosition
    @MatchId                  INT,
    @TeamId                   INT,
    @FormationPositionId      INT,
    @PlayerPositionId         INT,
    @ConditionOnMatch         INT,
    @QualityAtPositionOnMatch INT,
    @IsSuspended              BIT,
    @CreatedAt                DATETIME2(7),
    @ModifiedAt               DATETIME2(7),
    @CreatedBy                UNIQUEIDENTIFIER,
    @ModifiedBy               UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.MatchFormationPlayerPositions (MatchId, TeamId, FormationPositionId, PlayerPositionId, ConditionOnMatch, QualityAtPositionOnMatch, IsSuspended, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy)
    OUTPUT INSERTED.Id
    VALUES (@MatchId, @TeamId, @FormationPositionId, @PlayerPositionId, @ConditionOnMatch, @QualityAtPositionOnMatch, @IsSuspended, @CreatedAt, @ModifiedAt, @CreatedBy, @ModifiedBy);
END
