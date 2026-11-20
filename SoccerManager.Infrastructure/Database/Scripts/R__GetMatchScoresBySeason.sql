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
    -- columns are a contract with the Generator. The score rule itself lives in dbo.GetMatchTeamScore.
    SELECT
        mts.MatchId,
        m.CompetitionId,
        mts.LeagueTeamId,
        mts.IsHomeTeam,
        score.Goals,
        score.ShootoutGoals
    FROM dbo.Matches m
    INNER JOIN dbo.MatchTeamStatistics mts ON mts.MatchId = m.Id
    OUTER APPLY dbo.GetMatchTeamScore(mts.MatchId, mts.LeagueTeamId) score
    WHERE m.SeasonId = @SeasonId
    ORDER BY m.StartDateTime, m.Id, mts.IsHomeTeam DESC;
END
