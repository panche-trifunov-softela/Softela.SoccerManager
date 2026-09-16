CREATE OR ALTER PROCEDURE dbo.GetLeagueTeamManagerById
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, LeagueId, TeamId, ManagerId, StartDate, EndDate, IsCurrent, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy
    FROM dbo.LeagueTeamManagers
    WHERE Id = @Id;
END
