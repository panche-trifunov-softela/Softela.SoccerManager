CREATE OR ALTER PROCEDURE dbo.GetStandings
    @CompetitionId INT,
    @SeasonId      INT
AS
BEGIN
    SET NOCOUNT ON;

    -- Standard football tiebreak: points, then goal difference, then goals scored.
    -- Goal difference is computed here rather than stored as a column.
    SELECT Id, CompetitionId, SeasonId, TeamId, Points, GoalsFor, GoalsAgainst, Wins, Draws, Losses, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy
    FROM dbo.Standings
    WHERE CompetitionId = @CompetitionId AND SeasonId = @SeasonId
    ORDER BY Points DESC, (GoalsFor - GoalsAgainst) DESC, GoalsFor DESC;
END
