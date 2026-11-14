CREATE OR ALTER PROCEDURE dbo.UpdateMatchPlayerAbsence
    @Id           INT,
    @MatchId      INT,
    @LeagueTeamId INT,
    @PlayerId     INT,
    @Reason       TINYINT,
    @ModifiedAt   DATETIME2(7),
    @ModifiedBy   UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    -- The same guard as dbo.InsertMatchPlayerAbsence: an update can move the row to another match or club.
    IF NOT EXISTS (SELECT 1 FROM dbo.MatchTeamStatistics WHERE MatchId = @MatchId AND LeagueTeamId = @LeagueTeamId)
        THROW 50000, 'The club does not play in this match.', 1;

    UPDATE dbo.MatchPlayerAbsences
    SET MatchId      = @MatchId,
        LeagueTeamId = @LeagueTeamId,
        PlayerId     = @PlayerId,
        Reason       = @Reason,
        ModifiedAt   = @ModifiedAt,
        ModifiedBy   = @ModifiedBy
    WHERE Id = @Id;

    SELECT @Id;
END
