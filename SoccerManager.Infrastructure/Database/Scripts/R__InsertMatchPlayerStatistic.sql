CREATE OR ALTER PROCEDURE dbo.InsertMatchPlayerStatistic
    @PlayerId        INT,
    @MatchId         INT,
    @LeagueTeamId    INT,
    @Rating          DECIMAL(3,1),
    @IsStarter       BIT,
    @MinutesPlayed   INT,
    @Goals           INT,
    @OwnGoals        INT,
    @Assists         INT,
    @PenaltiesScored INT,
    @PenaltiesMissed INT,
    @YellowCards     INT,
    @RedCards        INT,
    @CreatedAt       DATETIME2(7),
    @ModifiedAt      DATETIME2(7),
    @CreatedBy       UNIQUEIDENTIFIER,
    @ModifiedBy      UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    -- The club must play the match: dbo.MatchTeamStatistics is the only record of a match's two clubs, and
    -- dbo.InsertMatchTeamStatistic already keeps its club in the match's league.
    IF NOT EXISTS (SELECT 1 FROM dbo.MatchTeamStatistics WHERE MatchId = @MatchId AND LeagueTeamId = @LeagueTeamId)
        THROW 50000, 'The club does not play in this match.', 1;

    INSERT INTO dbo.MatchPlayerStatistics (PlayerId, MatchId, LeagueTeamId, Rating, IsStarter, MinutesPlayed, Goals, OwnGoals, Assists, PenaltiesScored, PenaltiesMissed, YellowCards, RedCards, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy)
    OUTPUT INSERTED.Id
    VALUES (@PlayerId, @MatchId, @LeagueTeamId, @Rating, @IsStarter, @MinutesPlayed, @Goals, @OwnGoals, @Assists, @PenaltiesScored, @PenaltiesMissed, @YellowCards, @RedCards, @CreatedAt, @ModifiedAt, @CreatedBy, @ModifiedBy);
END
