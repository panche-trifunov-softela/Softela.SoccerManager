CREATE OR ALTER PROCEDURE dbo.InsertStanding
    @CompetitionId INT,
    @SeasonId      INT,
    @TeamId        INT,
    @Points        INT,
    @GoalsFor      INT,
    @GoalsAgainst  INT,
    @Wins          INT,
    @Draws         INT,
    @Losses        INT,
    @CreatedAt     DATETIME2(7),
    @ModifiedAt    DATETIME2(7),
    @CreatedBy     UNIQUEIDENTIFIER,
    @ModifiedBy    UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.Standings (CompetitionId, SeasonId, TeamId, Points, GoalsFor, GoalsAgainst, Wins, Draws, Losses, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy)
    OUTPUT INSERTED.Id
    VALUES (@CompetitionId, @SeasonId, @TeamId, @Points, @GoalsFor, @GoalsAgainst, @Wins, @Draws, @Losses, @CreatedAt, @ModifiedAt, @CreatedBy, @ModifiedBy);
END
