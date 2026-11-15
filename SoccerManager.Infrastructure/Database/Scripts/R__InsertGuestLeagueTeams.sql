-- Adds every Team that is not yet on a League's roster as a guest club: on the roster so its players can be bought, never managed.
-- Called when a League is created, after its division clubs are on the roster; a Team already on the roster is skipped.
CREATE OR ALTER PROCEDURE dbo.InsertGuestLeagueTeams
    @LeagueId  INT,
    @CreatedAt DATETIME2(7),
    @CreatedBy UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.LeagueTeams (LeagueId, TeamId, IsGuest, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy)
    SELECT @LeagueId, t.Id, 1, @CreatedAt, @CreatedAt, @CreatedBy, @CreatedBy
    FROM dbo.Teams t
    WHERE NOT EXISTS (SELECT 1 FROM dbo.LeagueTeams lt WHERE lt.LeagueId = @LeagueId AND lt.TeamId = t.Id);
END
