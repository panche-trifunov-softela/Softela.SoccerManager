CREATE OR ALTER PROCEDURE dbo.GetLeagueTeamManagerApplications
    @LeagueTeamId INT
AS
BEGIN
    SET NOCOUNT ON;

    -- Ordered by CreatedAt DESC so the newest application comes back first; Id DESC breaks a tie.
    SELECT Id, LeagueTeamId, ManagerId, Status, ResponseDate, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy
    FROM dbo.LeagueTeamManagerApplications
    WHERE LeagueTeamId = @LeagueTeamId
    ORDER BY CreatedAt DESC, Id DESC;
END
