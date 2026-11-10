CREATE OR ALTER PROCEDURE dbo.UpdatePenaltyAfterExtraTime
    @Id           INT,
    @MatchId      INT,
    @KickNumber   INT,
    @LeagueTeamId INT,
    @PlayerId     INT,
    @IsScored     BIT,
    @ModifiedAt   DATETIME2(7),
    @ModifiedBy   UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    -- The same guard as dbo.InsertPenaltyAfterExtraTime: an update can move the row to another match or club.
    IF NOT EXISTS (SELECT 1 FROM dbo.MatchTeamStatistics WHERE MatchId = @MatchId AND LeagueTeamId = @LeagueTeamId)
        THROW 50000, 'The club does not play in this match.', 1;

    UPDATE dbo.PenaltiesAfterExtraTime
    SET MatchId      = @MatchId,
        KickNumber   = @KickNumber,
        LeagueTeamId = @LeagueTeamId,
        PlayerId     = @PlayerId,
        IsScored     = @IsScored,
        ModifiedAt   = @ModifiedAt,
        ModifiedBy   = @ModifiedBy
    WHERE Id = @Id;

    SELECT @Id;
END
