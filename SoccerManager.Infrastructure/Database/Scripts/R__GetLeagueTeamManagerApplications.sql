CREATE OR ALTER PROCEDURE dbo.GetLeagueTeamManagerApplications
    @LeagueId INT,
    @TeamId   INT
AS
BEGIN
    SET NOCOUNT ON;

    -- Ordered by CreatedAt DESC so the newest application comes back first; Id DESC breaks a tie.
    SELECT Id, LeagueId, TeamId, ManagerId, Status, ResponseDate, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy
    FROM dbo.LeagueTeamManagerApplications
    WHERE LeagueId = @LeagueId AND TeamId = @TeamId
    ORDER BY CreatedAt DESC, Id DESC;
END
