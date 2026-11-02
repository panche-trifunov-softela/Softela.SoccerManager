-- The next match of one club, behind GET /api/league-teams/{id}/next-match: its earliest match that has not started (by start
-- time, then Id), with whether it plays at home and who the opponent is. The query handler checks access first.
-- A match belongs to a club through its dbo.MatchTeamStatistics row; a match has at most two (UQ_MatchTeamStatistics_MatchId_
-- IsHomeTeam), so the opponent LEFT JOIN yields at most one row and a match with only the club's own row has a NULL opponent.
-- An operator sets IsStarted, not the clock, so a match whose StartDateTime has passed stays next. No row = no match to come.
CREATE OR ALTER PROCEDURE dbo.GetNextMatchByLeagueTeam
    @LeagueTeamId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP (1)
           m.Id AS MatchId,
           m.StartDateTime,
           own.IsHomeTeam,
           opp.LeagueTeamId AS OpponentLeagueTeamId,
           t.Name AS OpponentTeamName,
           t.LogoUrl AS OpponentTeamLogoUrl
    FROM dbo.MatchTeamStatistics own
        INNER JOIN dbo.Matches m ON m.Id = own.MatchId
        LEFT JOIN dbo.MatchTeamStatistics opp ON opp.MatchId = m.Id AND opp.LeagueTeamId <> own.LeagueTeamId
        LEFT JOIN dbo.LeagueTeams olt ON olt.Id = opp.LeagueTeamId
        LEFT JOIN dbo.Teams t ON t.Id = olt.TeamId
    WHERE own.LeagueTeamId = @LeagueTeamId
      AND m.IsStarted = 0
    ORDER BY m.StartDateTime, m.Id;
END
