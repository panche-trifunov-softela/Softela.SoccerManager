CREATE OR ALTER PROCEDURE dbo.GetLeagueTeamManagers
    @LeagueId INT,
    @TeamId   INT
AS
BEGIN
    SET NOCOUNT ON;

    -- Ordered by StartDate DESC so the most recent appointment comes back first.
    SELECT Id, LeagueId, TeamId, ManagerId, StartDate, EndDate, IsCurrent, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy
    FROM dbo.LeagueTeamManagers
    WHERE LeagueId = @LeagueId AND TeamId = @TeamId
    ORDER BY StartDate DESC;
END
