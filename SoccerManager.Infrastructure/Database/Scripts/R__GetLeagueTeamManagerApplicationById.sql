CREATE OR ALTER PROCEDURE dbo.GetLeagueTeamManagerApplicationById
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, LeagueTeamId, ManagerId, Status, ResponseDate, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy
    FROM dbo.LeagueTeamManagerApplications
    WHERE Id = @Id;
END
