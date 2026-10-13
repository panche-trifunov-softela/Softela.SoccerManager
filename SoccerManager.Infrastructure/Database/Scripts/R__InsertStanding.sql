CREATE OR ALTER PROCEDURE dbo.InsertStanding
    @CompetitionId INT,
    @SeasonId      INT,
    @LeagueTeamId  INT,
    @Points        INT,
    @GoalsFor      INT,
    @GoalsAgainst  INT,
    @Wins          INT,
    @Draws         INT,
    @Losses        INT,
    @CreatedAt     DATETIME2(7),
    @ModifiedAt    DATETIME2(7),
    @CreatedBy     UNIQUEIDENTIFIER,
    @ModifiedBy    UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    -- The club, the season and the competition must all belong to one league. A row for another league's
    -- club would block that league's delete, since FK_Standings_LeagueTeams cannot cascade, and so would a
    -- row for another league's competition, through FK_Standings_Competitions.
    IF NOT EXISTS (
        SELECT 1
        FROM dbo.LeagueTeams lt
            INNER JOIN dbo.Seasons se ON se.LeagueId = lt.LeagueId
            INNER JOIN dbo.Competitions c ON c.LeagueId = lt.LeagueId
        WHERE lt.Id = @LeagueTeamId AND se.Id = @SeasonId AND c.Id = @CompetitionId)
        THROW 50000, 'The club, the season and the competition must belong to the same league.', 1;

    INSERT INTO dbo.Standings (CompetitionId, SeasonId, LeagueTeamId, Points, GoalsFor, GoalsAgainst, Wins, Draws, Losses, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy)
    OUTPUT INSERTED.Id
    VALUES (@CompetitionId, @SeasonId, @LeagueTeamId, @Points, @GoalsFor, @GoalsAgainst, @Wins, @Draws, @Losses, @CreatedAt, @ModifiedAt, @CreatedBy, @ModifiedBy);
END
