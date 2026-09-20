CREATE OR ALTER PROCEDURE dbo.UpdateMatchTeamTactic
    @Id                      INT,
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
    @ModifiedAt              DATETIME2(7),
    @ModifiedBy              UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.MatchTeamTactics
    SET MatchId                 = @MatchId,
        TeamId                  = @TeamId,
        Mentality               = @Mentality,
        Tempo                   = @Tempo,
        Passing                 = @Passing,
        Width                   = @Width,
        Pressing                = @Pressing,
        Tackling                = @Tackling,
        AttackingSide           = @AttackingSide,
        PenaltyTakerPlayerId    = @PenaltyTakerPlayerId,
        FreeKickTakerPlayerId   = @FreeKickTakerPlayerId,
        CornerKickTakerPlayerId = @CornerKickTakerPlayerId,
        CaptainPlayerId         = @CaptainPlayerId,
        ModifiedAt              = @ModifiedAt,
        ModifiedBy              = @ModifiedBy
    WHERE Id = @Id;

    SELECT @Id;
END
