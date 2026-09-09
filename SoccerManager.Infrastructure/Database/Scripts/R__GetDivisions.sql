CREATE OR ALTER PROCEDURE dbo.GetDivisions
    @LeagueId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, LeagueId, Name, [Order], TeamsPromoted, TeamsRelegated, TeamsInPlayoffs, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy
    FROM dbo.Divisions
    WHERE LeagueId = @LeagueId
    ORDER BY [Order];
END
