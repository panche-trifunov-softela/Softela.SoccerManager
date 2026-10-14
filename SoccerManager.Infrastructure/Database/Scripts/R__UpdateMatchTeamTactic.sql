CREATE OR ALTER PROCEDURE dbo.UpdateMatchTeamTactic
    @Id                      INT,
    @MatchId                 INT,
    @LeagueTeamId            INT,
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

    -- The same guard as dbo.InsertMatchTeamTactic: an update can move the row to another match or club.
    IF NOT EXISTS (
        SELECT 1
        FROM dbo.LeagueTeams lt
            INNER JOIN dbo.Seasons se ON se.LeagueId = lt.LeagueId
            INNER JOIN dbo.Matches m ON m.SeasonId = se.Id
        WHERE lt.Id = @LeagueTeamId AND m.Id = @MatchId)
        THROW 50000, 'The club and the match must belong to the same league.', 1;

    UPDATE dbo.MatchTeamTactics
    SET MatchId                 = @MatchId,
        LeagueTeamId            = @LeagueTeamId,
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
