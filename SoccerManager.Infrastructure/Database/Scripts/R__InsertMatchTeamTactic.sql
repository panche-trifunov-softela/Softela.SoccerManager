CREATE OR ALTER PROCEDURE dbo.InsertMatchTeamTactic
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
    @CreatedAt               DATETIME2(7),
    @ModifiedAt              DATETIME2(7),
    @CreatedBy               UNIQUEIDENTIFIER,
    @ModifiedBy              UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    -- The club must be on the roster of the match's league (its season's league). A row for another
    -- league's club would block that league's delete, since FK_MatchTeamTactics_LeagueTeams
    -- cannot cascade.
    IF NOT EXISTS (
        SELECT 1
        FROM dbo.LeagueTeams lt
            INNER JOIN dbo.Seasons se ON se.LeagueId = lt.LeagueId
            INNER JOIN dbo.Matches m ON m.SeasonId = se.Id
        WHERE lt.Id = @LeagueTeamId AND m.Id = @MatchId)
        THROW 50000, 'The club and the match must belong to the same league.', 1;

    INSERT INTO dbo.MatchTeamTactics (MatchId, LeagueTeamId, Mentality, Tempo, Passing, Width, Pressing, Tackling, AttackingSide, PenaltyTakerPlayerId, FreeKickTakerPlayerId, CornerKickTakerPlayerId, CaptainPlayerId, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy)
    OUTPUT INSERTED.Id
    VALUES (@MatchId, @LeagueTeamId, @Mentality, @Tempo, @Passing, @Width, @Pressing, @Tackling, @AttackingSide, @PenaltyTakerPlayerId, @FreeKickTakerPlayerId, @CornerKickTakerPlayerId, @CaptainPlayerId, @CreatedAt, @ModifiedAt, @CreatedBy, @ModifiedBy);
END
