CREATE OR ALTER PROCEDURE dbo.InsertMatchTeamTactic
    @MatchId                 INT,
    @TeamId                  INT,
    @Mentality               TINYINT,
    @Tempo                   TINYINT,
    @Passing                 TINYINT,
    @Width                   TINYINT,
    @Pressing                TINYINT,
    @Tackling                TINYINT,
    @AttackingSide           TINYINT,
    @PenaltyTakerPlayerId    INT,
    @FreeKickTakerPlayerId   INT,
    @CornerKickTakerPlayerId INT,
    @CaptainPlayerId         INT,
    @CreatedAt               DATETIME2(7),
    @ModifiedAt              DATETIME2(7),
    @CreatedBy               UNIQUEIDENTIFIER,
    @ModifiedBy              UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.MatchTeamTactics (MatchId, TeamId, Mentality, Tempo, Passing, Width, Pressing, Tackling, AttackingSide, PenaltyTakerPlayerId, FreeKickTakerPlayerId, CornerKickTakerPlayerId, CaptainPlayerId, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy)
    OUTPUT INSERTED.Id
    VALUES (@MatchId, @TeamId, @Mentality, @Tempo, @Passing, @Width, @Pressing, @Tackling, @AttackingSide, @PenaltyTakerPlayerId, @FreeKickTakerPlayerId, @CornerKickTakerPlayerId, @CaptainPlayerId, @CreatedAt, @ModifiedAt, @CreatedBy, @ModifiedBy);
END
