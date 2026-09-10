CREATE OR ALTER PROCEDURE dbo.GetStandings
    @SeasonId   INT,
    @DivisionId INT
AS
BEGIN
    SET NOCOUNT ON;

    -- Standard football tiebreak: points, then goal difference, then goals scored.
    -- Goal difference is computed here rather than stored as a column.
    SELECT Id, SeasonId, DivisionId, TeamId, Points, GoalsFor, GoalsAgainst, Wins, Draws, Losses, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy
    FROM dbo.Standings
    WHERE SeasonId = @SeasonId AND DivisionId = @DivisionId
    ORDER BY Points DESC, (GoalsFor - GoalsAgainst) DESC, GoalsFor DESC;
END
