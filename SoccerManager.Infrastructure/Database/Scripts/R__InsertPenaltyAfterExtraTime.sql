CREATE OR ALTER PROCEDURE dbo.InsertPenaltyAfterExtraTime
    @MatchId      INT,
    @KickNumber   INT,
    @LeagueTeamId INT,
    @PlayerId     INT,
    @IsScored     BIT,
    @CreatedAt    DATETIME2(7),
    @ModifiedAt   DATETIME2(7),
    @CreatedBy    UNIQUEIDENTIFIER,
    @ModifiedBy   UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    -- The club must play the match: dbo.MatchTeamStatistics is the only record of a match's two clubs, and
    -- dbo.InsertMatchTeamStatistic already keeps its club in the match's league.
    IF NOT EXISTS (SELECT 1 FROM dbo.MatchTeamStatistics WHERE MatchId = @MatchId AND LeagueTeamId = @LeagueTeamId)
        THROW 50000, 'The club does not play in this match.', 1;

    INSERT INTO dbo.PenaltiesAfterExtraTime (MatchId, KickNumber, LeagueTeamId, PlayerId, IsScored, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy)
    OUTPUT INSERTED.Id
    VALUES (@MatchId, @KickNumber, @LeagueTeamId, @PlayerId, @IsScored, @CreatedAt, @ModifiedAt, @CreatedBy, @ModifiedBy);
END
