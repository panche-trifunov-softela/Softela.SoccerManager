-- The squad of one league team as the lineup logic reads it: one flat row per registered player and position rating, for the
-- caller to group by player; a player with no position rating is one row with NULL position columns.
-- A player can be registered twice in one club (V1_0_0_37 dropped the unique key), so the oldest registration (lowest Id) is used,
-- as in dbo.GetLeagueTeamPlayerByLeagueAndPlayer. The current rating (IsCurrent = 1) is a LEFT JOIN. Rows are ordered by player,
-- then highest Quality, then lowest Id (dbo.GetPlayerMainPosition's order), so a player's first row is his main position.
-- IsSuspended is the domestic competition flag, the one lineups read.
CREATE OR ALTER PROCEDURE dbo.GetLeagueTeamLineupCandidates
    @LeagueTeamId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT reg.PlayerId,
           reg.SquadNumber,
           reg.Condition,
           reg.IsSuspendedDomesticCompetition AS IsSuspended,
           cr.Rating,
           pp.Id AS PlayerPositionId,
           pp.PositionId,
           pp.Quality,
           pos.Area AS PositionArea
    FROM (
        SELECT ltp.PlayerId,
               ltp.SquadNumber,
               ltp.Condition,
               ltp.IsSuspendedDomesticCompetition,
               ROW_NUMBER() OVER (PARTITION BY ltp.PlayerId ORDER BY ltp.Id) AS RegistrationOrder
        FROM dbo.LeagueTeamPlayers ltp
        WHERE ltp.LeagueTeamId = @LeagueTeamId
    ) reg
        LEFT JOIN dbo.PlayerRatings cr ON cr.PlayerId = reg.PlayerId AND cr.IsCurrent = 1
        LEFT JOIN dbo.PlayerPositions pp ON pp.PlayerId = reg.PlayerId
        LEFT JOIN dbo.Positions pos ON pos.Id = pp.PositionId
    WHERE reg.RegistrationOrder = 1
    ORDER BY reg.PlayerId, pp.Quality DESC, pp.Id;
END
