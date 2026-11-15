-- Every squad player of every club on one League's roster, each with the player's current rating (NULL when the player has none).
-- The Generator ranks a League's division clubs and guests by squad strength from this one read, instead of one squad read per club.
-- The current rating is the dbo.PlayerRatings row with IsCurrent = 1 (at most one by UX_PlayerRatings_PlayerId_Current), a LEFT JOIN as in dbo.GetLeagueTeamSquad.
-- Rows are kept as stored (no DISTINCT), so a player listed twice still reaches the Generator's own check.
-- This never writes. No C# in this repository calls it: the Generator does, so its columns are a contract with the Generator.
CREATE OR ALTER PROCEDURE dbo.GetLeagueSquadRatings
    @LeagueId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT lt.Id AS LeagueTeamId,
           ltp.PlayerId,
           cr.Rating
    FROM dbo.LeagueTeams lt
        INNER JOIN dbo.LeagueTeamPlayers ltp ON ltp.LeagueTeamId = lt.Id
        LEFT JOIN dbo.PlayerRatings cr ON cr.PlayerId = ltp.PlayerId AND cr.IsCurrent = 1
    WHERE lt.LeagueId = @LeagueId
    ORDER BY lt.Id,
             ltp.PlayerId,
             ltp.Id;
END
