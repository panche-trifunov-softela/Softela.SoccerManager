CREATE OR ALTER PROCEDURE dbo.GetMatchScoresBySeason
    @SeasonId INT
AS
BEGIN
    SET NOCOUNT ON;

    -- One row per club per match of the season, for the Generator's playoffs and rollover. Goals is the club's
    -- players' goals plus the opponent's players' own goals, and 0 for a match with no player rows, so a reader
    -- tells an unplayed match from a 0-0 by IsFinished. ShootoutGoals is NULL when the match had no shootout.
    -- The opponent comes from the match's other dbo.MatchTeamStatistics row. Friendlies are included, with a
    -- NULL CompetitionId. This never writes. No C# in this repository calls it: the Generator does, so its
    -- columns are a contract with the Generator.
    SELECT
        mts.MatchId,
        m.CompetitionId,
        mts.LeagueTeamId,
        mts.IsHomeTeam,
        ISNULL(own.Goals, 0) + ISNULL(opp.OwnGoals, 0) AS Goals,
        CASE WHEN shootout.Kicks > 0 THEN scored.Scored END AS ShootoutGoals
    FROM dbo.Matches m
    INNER JOIN dbo.MatchTeamStatistics mts ON mts.MatchId = m.Id
    OUTER APPLY (
        SELECT SUM(p.Goals) AS Goals
        FROM dbo.MatchPlayerStatistics p
        WHERE p.MatchId = mts.MatchId AND p.LeagueTeamId = mts.LeagueTeamId
    ) own
    OUTER APPLY (
        SELECT SUM(p.OwnGoals) AS OwnGoals
        FROM dbo.MatchTeamStatistics o
        INNER JOIN dbo.MatchPlayerStatistics p ON p.MatchId = o.MatchId AND p.LeagueTeamId = o.LeagueTeamId
        WHERE o.MatchId = mts.MatchId AND o.LeagueTeamId <> mts.LeagueTeamId
    ) opp
    OUTER APPLY (
        SELECT COUNT(*) AS Kicks
        FROM dbo.PenaltiesAfterExtraTime k
        WHERE k.MatchId = mts.MatchId
    ) shootout
    OUTER APPLY (
        SELECT COUNT(*) AS Scored
        FROM dbo.PenaltiesAfterExtraTime k
        WHERE k.MatchId = mts.MatchId AND k.LeagueTeamId = mts.LeagueTeamId AND k.IsScored = 1
    ) scored
    WHERE m.SeasonId = @SeasonId
    ORDER BY m.StartDateTime, m.Id, mts.IsHomeTeam DESC;
END
