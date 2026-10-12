CREATE OR ALTER PROCEDURE dbo.GetLeagueTeams
    @LeagueId INT
AS
BEGIN
    SET NOCOUNT ON;

    -- Ordered by Id so the roster comes back in a stable order.
    SELECT Id, LeagueId, TeamId, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy
    FROM dbo.LeagueTeams
    WHERE LeagueId = @LeagueId
    ORDER BY Id;
END
