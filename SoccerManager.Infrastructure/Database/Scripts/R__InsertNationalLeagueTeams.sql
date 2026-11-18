-- Puts every national team that is not yet on a League's roster on it, for the League's national competition (its World Cup). CreateCompetition calls it when the competition it creates is national; a national team already on the roster is skipped, so calling it again adds only national teams created since.
-- A national team row is never a guest and has no squad (V1_0_0_68).
CREATE OR ALTER PROCEDURE dbo.InsertNationalLeagueTeams
    @LeagueId  INT,
    @CreatedAt DATETIME2(7),
    @CreatedBy UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.LeagueTeams (LeagueId, TeamId, NationalTeamId, IsGuest, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy)
    SELECT @LeagueId, NULL, nt.Id, 0, @CreatedAt, @CreatedAt, @CreatedBy, @CreatedBy
    FROM dbo.NationalTeams nt
    WHERE NOT EXISTS (SELECT 1 FROM dbo.LeagueTeams lt WHERE lt.LeagueId = @LeagueId AND lt.NationalTeamId = nt.Id);
END
