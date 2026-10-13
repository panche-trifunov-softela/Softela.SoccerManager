CREATE OR ALTER PROCEDURE dbo.GetLeagueTeamManagers
    @LeagueTeamId INT
AS
BEGIN
    SET NOCOUNT ON;

    -- Ordered by StartDate DESC so the most recent appointment comes back first.
    SELECT Id, LeagueTeamId, ManagerId, StartDate, EndDate, IsCurrent, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy
    FROM dbo.LeagueTeamManagers
    WHERE LeagueTeamId = @LeagueTeamId
    ORDER BY StartDate DESC;
END
