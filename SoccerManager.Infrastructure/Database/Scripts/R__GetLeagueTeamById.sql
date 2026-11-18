-- National team rows (V1_0_0_68) are left out: this returns the clubs on the roster.
CREATE OR ALTER PROCEDURE dbo.GetLeagueTeamById
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, LeagueId, TeamId, IsGuest, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy
    FROM dbo.LeagueTeams
    WHERE Id = @Id
      AND TeamId IS NOT NULL;
END
