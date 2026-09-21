CREATE OR ALTER PROCEDURE dbo.UpdateMatchFormationPlayerPosition
    @Id                       INT,
    @MatchId                  INT,
    @TeamId                   INT,
    @FormationPositionId      INT,
    @PlayerPositionId         INT,
    @ConditionOnMatch         INT,
    @QualityAtPositionOnMatch INT,
    @IsSuspended              BIT,
    @ModifiedAt               DATETIME2(7),
    @ModifiedBy               UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.MatchFormationPlayerPositions
    SET MatchId                  = @MatchId,
        TeamId                   = @TeamId,
        FormationPositionId      = @FormationPositionId,
        PlayerPositionId         = @PlayerPositionId,
        ConditionOnMatch         = @ConditionOnMatch,
        QualityAtPositionOnMatch = @QualityAtPositionOnMatch,
        IsSuspended              = @IsSuspended,
        ModifiedAt               = @ModifiedAt,
        ModifiedBy               = @ModifiedBy
    WHERE Id = @Id;

    SELECT @Id;
END
