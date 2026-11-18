-- National team rows (V1_0_0_68) are left out: this returns the clubs on the roster.
CREATE OR ALTER PROCEDURE dbo.GetLeagueTeams
    @LeagueId INT
AS
BEGIN
    SET NOCOUNT ON;

    -- Ordered by Id so the roster comes back in a stable order.
    SELECT Id, LeagueId, TeamId, IsGuest, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy
    FROM dbo.LeagueTeams
    WHERE LeagueId = @LeagueId
      AND TeamId IS NOT NULL
    ORDER BY Id;
END
