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

    -- A national team is on a League's roster to play its national competition (V1_0_0_68); a national standing waits for national team management.
    IF EXISTS (SELECT 1 FROM dbo.LeagueTeams WHERE Id = @LeagueTeamId AND NationalTeamId IS NOT NULL)
        THROW 50000, 'A national team has no standings.', 1;

    -- A national competition's group tables are worked out from its matches; nothing stores them as standings.
    IF EXISTS (SELECT 1 FROM dbo.Competitions WHERE Id = @CompetitionId AND IsNational = 1)
        THROW 50000, 'A national competition has no standings.', 1;

    -- The same guard as dbo.InsertStanding: an update can move the row to another competition, season or club.
    IF NOT EXISTS (
        SELECT 1
        FROM dbo.LeagueTeams lt
            INNER JOIN dbo.Seasons se ON se.LeagueId = lt.LeagueId
            INNER JOIN dbo.Competitions c ON c.LeagueId = lt.LeagueId
        WHERE lt.Id = @LeagueTeamId AND se.Id = @SeasonId AND c.Id = @CompetitionId)
        THROW 50000, 'The club, the season and the competition must belong to the same league.', 1;

    -- A guest may play cups (Order 0) but never a division.
    IF EXISTS (
        SELECT 1
        FROM dbo.LeagueTeams lt
            INNER JOIN dbo.Competitions c ON c.Id = @CompetitionId
        WHERE lt.Id = @LeagueTeamId AND lt.IsGuest = 1 AND c.[Order] > 0)
        THROW 50000, 'A guest club cannot have a standing in a division.', 1;

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
