CREATE OR ALTER PROCEDURE dbo.UpdateMatchPlayerStatistic
    @Id              INT,
    @PlayerId        INT,
    @MatchId         INT,
    @Rating          DECIMAL(3,1),
    @IsStarter       BIT,
    @MinutesPlayed   INT,
    @Goals           INT,
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

    UPDATE dbo.MatchPlayerStatistics
    SET PlayerId        = @PlayerId,
        MatchId         = @MatchId,
        Rating          = @Rating,
        IsStarter       = @IsStarter,
        MinutesPlayed   = @MinutesPlayed,
        Goals           = @Goals,
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
