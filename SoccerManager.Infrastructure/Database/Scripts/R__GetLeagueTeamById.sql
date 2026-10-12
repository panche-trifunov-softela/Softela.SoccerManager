CREATE OR ALTER PROCEDURE dbo.GetLeagueTeamById
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, LeagueId, TeamId, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy
    FROM dbo.LeagueTeams
    WHERE Id = @Id;
END
