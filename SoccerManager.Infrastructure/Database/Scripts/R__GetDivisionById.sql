CREATE OR ALTER PROCEDURE dbo.GetDivisionById
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, LeagueId, Name, [Order], TeamsPromoted, TeamsRelegated, TeamsInPlayoffs, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy
    FROM dbo.Divisions
    WHERE Id = @Id;
END
