CREATE OR ALTER PROCEDURE dbo.GetStandingById
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, SeasonId, DivisionId, TeamId, Points, GoalsFor, GoalsAgainst, Wins, Draws, Losses, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy
    FROM dbo.Standings
    WHERE Id = @Id;
END
