CREATE OR ALTER PROCEDURE dbo.InsertMatchPlayerStatistic
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
    @CreatedAt       DATETIME2(7),
    @ModifiedAt      DATETIME2(7),
    @CreatedBy       UNIQUEIDENTIFIER,
    @ModifiedBy      UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.MatchPlayerStatistics (PlayerId, MatchId, Rating, IsStarter, MinutesPlayed, Goals, Assists, PenaltiesScored, PenaltiesMissed, YellowCards, RedCards, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy)
    OUTPUT INSERTED.Id
    VALUES (@PlayerId, @MatchId, @Rating, @IsStarter, @MinutesPlayed, @Goals, @Assists, @PenaltiesScored, @PenaltiesMissed, @YellowCards, @RedCards, @CreatedAt, @ModifiedAt, @CreatedBy, @ModifiedBy);
END
