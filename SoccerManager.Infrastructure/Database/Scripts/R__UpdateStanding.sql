CREATE OR ALTER PROCEDURE dbo.UpdateStanding
    @Id            INT,
    @CompetitionId INT,
    @SeasonId      INT,
    @TeamId        INT,
    @Points        INT,
    @GoalsFor      INT,
    @GoalsAgainst  INT,
    @Wins          INT,
    @Draws         INT,
    @Losses        INT,
    @ModifiedAt    DATETIME2(7),
    @ModifiedBy    UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.Standings
    SET CompetitionId = @CompetitionId,
        SeasonId      = @SeasonId,
        TeamId        = @TeamId,
        Points        = @Points,
        GoalsFor      = @GoalsFor,
        GoalsAgainst  = @GoalsAgainst,
        Wins          = @Wins,
        Draws         = @Draws,
        Losses        = @Losses,
        ModifiedAt    = @ModifiedAt,
        ModifiedBy    = @ModifiedBy
    WHERE Id = @Id;

    SELECT @Id;
END
