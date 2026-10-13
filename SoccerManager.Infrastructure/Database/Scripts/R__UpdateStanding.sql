CREATE OR ALTER PROCEDURE dbo.UpdateStanding
    @Id            INT,
    @CompetitionId INT,
    @SeasonId      INT,
    @LeagueTeamId  INT,
    @Points        INT,
    @GoalsFor      INT,
    @GoalsAgainst  INT,
    @Wins          INT,
    @Draws         INT,
    @Losses        INT,
    @ModifiedAt    DATETIME2(7),
    @ModifiedBy    UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    -- The same guard as dbo.InsertStanding: an update can move the row to another competition, season or club.
    IF NOT EXISTS (
        SELECT 1
        FROM dbo.LeagueTeams lt
            INNER JOIN dbo.Seasons se ON se.LeagueId = lt.LeagueId
            INNER JOIN dbo.Competitions c ON c.LeagueId = lt.LeagueId
        WHERE lt.Id = @LeagueTeamId AND se.Id = @SeasonId AND c.Id = @CompetitionId)
        THROW 50000, 'The club, the season and the competition must belong to the same league.', 1;

    UPDATE dbo.Standings
    SET CompetitionId = @CompetitionId,
        SeasonId      = @SeasonId,
        LeagueTeamId  = @LeagueTeamId,
        Points        = @Points,
        GoalsFor      = @GoalsFor,
        GoalsAgainst  = @GoalsAgainst,
        Wins          = @Wins,
        Draws         = @Draws,
        Losses        = @Losses,
        ModifiedAt    = @ModifiedAt,
        ModifiedBy    = @ModifiedBy
    WHERE Id = @Id;

    SELECT @Id;
END
