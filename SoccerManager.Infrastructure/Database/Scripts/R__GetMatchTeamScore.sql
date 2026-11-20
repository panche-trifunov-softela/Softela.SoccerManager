-- One club's score in one match: Goals is the club's players' goals plus the opponent's players' own goals, and 0 for a match with
-- no player rows, so a reader tells an unplayed match from a 0-0 by the match's IsFinished. ShootoutGoals is the club's scored
-- shootout kicks, and NULL when the match had no shootout. The opponent is the match's other dbo.MatchTeamStatistics row; a NULL
-- @LeagueTeamId (a match with no second participant) scores 0.
-- An inline table-valued function, so SQL Server expands it into the calling query like a view; callers OUTER APPLY it, and it
-- always yields exactly one row. No SCHEMABINDING, so a later migration can still alter the tables it reads.
CREATE OR ALTER FUNCTION dbo.GetMatchTeamScore
(
    @MatchId INT,
    @LeagueTeamId INT
)
RETURNS TABLE
AS
RETURN
(
    SELECT
        ISNULL(own.Goals, 0) + ISNULL(opp.OwnGoals, 0) AS Goals,
        CASE WHEN shootout.Kicks > 0 THEN scored.Scored END AS ShootoutGoals
    FROM (
        SELECT SUM(p.Goals) AS Goals
        FROM dbo.MatchPlayerStatistics p
        WHERE p.MatchId = @MatchId AND p.LeagueTeamId = @LeagueTeamId
    ) own
    CROSS JOIN (
        SELECT SUM(p.OwnGoals) AS OwnGoals
        FROM dbo.MatchTeamStatistics o
        INNER JOIN dbo.MatchPlayerStatistics p ON p.MatchId = o.MatchId AND p.LeagueTeamId = o.LeagueTeamId
        WHERE o.MatchId = @MatchId AND o.LeagueTeamId <> @LeagueTeamId
    ) opp
    CROSS JOIN (
        SELECT COUNT(*) AS Kicks
        FROM dbo.PenaltiesAfterExtraTime k
        WHERE k.MatchId = @MatchId
    ) shootout
    CROSS JOIN (
        SELECT COUNT(*) AS Scored
        FROM dbo.PenaltiesAfterExtraTime k
        WHERE k.MatchId = @MatchId AND k.LeagueTeamId = @LeagueTeamId AND k.IsScored = 1
    ) scored
);
