CREATE OR ALTER PROCEDURE dbo.InsertStanding
    @SeasonId     INT,
    @DivisionId   INT,
    @TeamId       INT,
    @Points       INT,
    @GoalsFor     INT,
    @GoalsAgainst INT,
    @Wins         INT,
    @Draws        INT,
    @Losses       INT,
    @CreatedAt    DATETIME2(7),
    @ModifiedAt   DATETIME2(7),
    @CreatedBy    UNIQUEIDENTIFIER,
    @ModifiedBy   UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.Standings (SeasonId, DivisionId, TeamId, Points, GoalsFor, GoalsAgainst, Wins, Draws, Losses, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy)
    OUTPUT INSERTED.Id
    VALUES (@SeasonId, @DivisionId, @TeamId, @Points, @GoalsFor, @GoalsAgainst, @Wins, @Draws, @Losses, @CreatedAt, @ModifiedAt, @CreatedBy, @ModifiedBy);
END
