-- Every player who has a national team, with that team and the player's current rating (NULL when the player has none).
-- A national squad is every player of that nationality in the database, so the Generator ranks the nations for the World Cup qualifiers from this one read.
-- The current rating is the dbo.PlayerRatings row with IsCurrent = 1 (at most one by UX_PlayerRatings_PlayerId_Current), a LEFT JOIN as in dbo.GetLeagueSquadRatings.
-- This never writes. No C# in this repository calls it: the Generator does, so its columns are a contract with the Generator.
CREATE OR ALTER PROCEDURE dbo.GetNationalTeamPlayerRatings
AS
BEGIN
    SET NOCOUNT ON;

    SELECT p.NationalTeamId,
           cr.Rating
    FROM dbo.Players p
        LEFT JOIN dbo.PlayerRatings cr ON cr.PlayerId = p.Id AND cr.IsCurrent = 1
    WHERE p.NationalTeamId IS NOT NULL
    ORDER BY p.NationalTeamId,
             cr.Rating DESC;
END
