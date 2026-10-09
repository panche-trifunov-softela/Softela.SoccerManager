CREATE OR ALTER PROCEDURE dbo.GetLeagueTeamManagerApplicationById
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, LeagueId, TeamId, ManagerId, Status, ResponseDate, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy
    FROM dbo.LeagueTeamManagerApplications
    WHERE Id = @Id;
END
