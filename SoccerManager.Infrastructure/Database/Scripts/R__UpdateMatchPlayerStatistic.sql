CREATE OR ALTER PROCEDURE dbo.UpdateMatchPlayerStatistic
    @Id              INT,
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
    @ModifiedAt      DATETIME2(7),
    @ModifiedBy      UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    -- The same guard as dbo.InsertMatchPlayerStatistic: an update can move the row to another match or club.
    IF NOT EXISTS (SELECT 1 FROM dbo.MatchTeamStatistics WHERE MatchId = @MatchId AND LeagueTeamId = @LeagueTeamId)
        THROW 50000, 'The club does not play in this match.', 1;

    UPDATE dbo.MatchPlayerStatistics
    SET PlayerId        = @PlayerId,
        MatchId         = @MatchId,
        LeagueTeamId    = @LeagueTeamId,
        Rating          = @Rating,
        IsStarter       = @IsStarter,
        MinutesPlayed   = @MinutesPlayed,
        Goals           = @Goals,
        OwnGoals        = @OwnGoals,
        Assists         = @Assists,
        PenaltiesScored = @PenaltiesScored,
        PenaltiesMissed = @PenaltiesMissed,
        YellowCards     = @YellowCards,
        RedCards        = @RedCards,
        ModifiedAt      = @ModifiedAt,
        ModifiedBy      = @ModifiedBy
    WHERE Id = @Id;

    SELECT @Id;
END
