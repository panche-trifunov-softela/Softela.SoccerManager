-- The schedule of one club, behind GET /api/league-teams/{id}/schedule: every match it plays in its League's latest season,
-- earliest first (by start time, then Id), finished or not, with whether it plays at home, the competition and round, and who the
-- opponent is. The latest season is the one with the highest SeasonNumber, as dbo.GetLatestSeason reads it: a season has no date
-- or current flag. An unknown club, or a club whose League has no season, gives no rows. No access check: every signed-in user
-- may read it.
-- A match belongs to a club through its dbo.MatchTeamStatistics row; a match has at most two (UQ_MatchTeamStatistics_MatchId_
-- IsHomeTeam), so the opponent LEFT JOIN yields at most one row and a match with only the club's own row has a NULL opponent. A
-- nation opponent has no name here, as only nations' rows play nations.
-- The goals come from dbo.GetMatchTeamScore for each side and mean something only when IsFinished is set: an unplayed match scores
-- 0-0 here. A friendly has a NULL competition. This never writes.
CREATE OR ALTER PROCEDURE dbo.GetScheduleByLeagueTeam
    @LeagueTeamId INT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @SeasonId INT =
    (
        SELECT TOP (1) s.Id
        FROM dbo.LeagueTeams lt
            INNER JOIN dbo.Seasons s ON s.LeagueId = lt.LeagueId
        WHERE lt.Id = @LeagueTeamId
        ORDER BY s.SeasonNumber DESC
    );

    SELECT m.Id AS MatchId,
           m.StartDateTime,
           own.IsHomeTeam,
           m.IsFinished,
           c.Name AS CompetitionName,
           m.Stage,
           m.RoundNumber,
           m.Leg,
           t.Name AS OpponentTeamName,
           t.LogoUrl AS OpponentTeamLogoUrl,
           us.Goals AS GoalsFor,
           them.Goals AS GoalsAgainst,
           us.ShootoutGoals AS ShootoutGoalsFor,
           them.ShootoutGoals AS ShootoutGoalsAgainst
    FROM dbo.MatchTeamStatistics own
        INNER JOIN dbo.Matches m ON m.Id = own.MatchId
        LEFT JOIN dbo.Competitions c ON c.Id = m.CompetitionId
        LEFT JOIN dbo.MatchTeamStatistics opp ON opp.MatchId = m.Id AND opp.LeagueTeamId <> own.LeagueTeamId
        LEFT JOIN dbo.LeagueTeams olt ON olt.Id = opp.LeagueTeamId
        LEFT JOIN dbo.Teams t ON t.Id = olt.TeamId
        OUTER APPLY dbo.GetMatchTeamScore(m.Id, own.LeagueTeamId) us
        OUTER APPLY dbo.GetMatchTeamScore(m.Id, opp.LeagueTeamId) them
    WHERE own.LeagueTeamId = @LeagueTeamId
      AND m.SeasonId = @SeasonId
    ORDER BY m.StartDateTime, m.Id;
END
