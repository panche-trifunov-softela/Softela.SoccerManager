-- The seven instructions of one club for one match, in the order Mentality, Tempo, Passing, Width, Pressing, Tackling,
-- AttackingSide, behind GET /api/league-teams/{id}/next-match and the first save of PUT /api/matches/{id}/instructions (so a
-- page shows what the first change saves). Always exactly one row, from the first of these that exists:
--   1. the club's own dbo.MatchTeamTactics row for the match;
--   2. the club's row for its most recent earlier match, earlier meaning by start time, then Id, the order of
--      dbo.GetNextMatchByLeagueTeam; a later match's row and another club's rows are never used;
--   3. the defaults: Mentality Balanced (2), Tempo Normal (2), Passing Mixed (4), Width Balanced (2), Pressing Balanced (2),
--      Tackling Normal (2), AttackingSide Mixed (1).
-- An unknown match finds nothing in steps 1 and 2, so it gets the defaults. It never writes.
CREATE OR ALTER PROCEDURE dbo.GetMatchTeamTacticInstructions
    @MatchId      INT,
    @LeagueTeamId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP (1)
           c.Mentality,
           c.Tempo,
           c.Passing,
           c.Width,
           c.Pressing,
           c.Tackling,
           c.AttackingSide
    FROM (
        SELECT 0 AS Priority,
               CAST(NULL AS DATETIME2(7)) AS StartDateTime,
               t.MatchId AS SourceMatchId,
               t.Mentality,
               t.Tempo,
               t.Passing,
               t.Width,
               t.Pressing,
               t.Tackling,
               t.AttackingSide
        FROM dbo.MatchTeamTactics t
        WHERE t.MatchId = @MatchId AND t.LeagueTeamId = @LeagueTeamId

        UNION ALL

        SELECT 1,
               m.StartDateTime,
               m.Id,
               t.Mentality,
               t.Tempo,
               t.Passing,
               t.Width,
               t.Pressing,
               t.Tackling,
               t.AttackingSide
        FROM dbo.MatchTeamTactics t
            INNER JOIN dbo.Matches m ON m.Id = t.MatchId
            INNER JOIN dbo.Matches cur ON cur.Id = @MatchId
        WHERE t.LeagueTeamId = @LeagueTeamId
          AND (m.StartDateTime < cur.StartDateTime OR (m.StartDateTime = cur.StartDateTime AND m.Id < cur.Id))

        UNION ALL

        SELECT 2,
               CAST(NULL AS DATETIME2(7)),
               0,
               CAST(2 AS TINYINT),
               CAST(2 AS TINYINT),
               CAST(4 AS TINYINT),
               CAST(2 AS TINYINT),
               CAST(2 AS TINYINT),
               CAST(2 AS TINYINT),
               CAST(1 AS TINYINT)
    ) c
    ORDER BY c.Priority, c.StartDateTime DESC, c.SourceMatchId DESC;
END
