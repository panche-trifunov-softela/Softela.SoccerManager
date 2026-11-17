-- The players and the clubs of a league whose names contain a text, behind GET /api/league-teams/{id}/search, as four result
-- sets in this order: the player count, the best @Take players, the club count, the best @Take clubs. It does not check
-- access: the query handler does that first, through IClubAccess.
-- CHARINDEX matches the term literally under an accent- and case-insensitive collation, so a term typed without accents
-- still finds an accented name whatever the database's own collation, and the LIKE wildcards % and _ and the bracket mean
-- nothing in it; the caller passes the term trimmed and at least 2 characters long.
-- The league is the league of @LeagueTeamId. Players are every dbo.Players row that matches, registered at a club of that
-- league or not. A player's club is the registration at a club of that league, picked as dbo.GetLeaguePlayerProfile picks
-- it: @LeagueTeamId's own first, else the lowest dbo.LeagueTeamPlayers.Id (V1_0_0_37 dropped the unique key, so a player can
-- be registered twice; the TOP (1) shows such a player once). A player with no registration at any club of the league is a
-- free agent, with a NULL LeagueTeamId and TeamName: never registered, or released (the registration is deleted), or
-- registered only in another league. Clubs are every club on the league's roster, guests and the anchor itself included.
-- The rating is the player's current dbo.PlayerRatings row (at most one by UX_PlayerRatings_PlayerId_Current), a LEFT JOIN, so
-- an unrated player is still found, with a NULL Rating. The main position comes from dbo.GetPlayerMainPosition, as in
-- dbo.GetLeagueTeamSquad; a player with none gets NULL names, which the Dapper model leaves as empty strings.
-- Order, players: names where the term starts the name or a word in it first, then rating, best first (NULL sorts last under
-- DESC), then name, then Id. Clubs: word start first, then name, then LeagueTeamId. A word starts after a space, a hyphen, an
-- apostrophe or the typographic apostrophe (U+2019, written NCHAR(8217) so that the script stays ASCII). Each separator is its
-- own CHARINDEX with the separator in front of the term, never LIKE. Under Latin1_General_100_CI_AI the hyphen and the
-- apostrophe are not ignored: tried on the engine, '-' + 'ilva' is not in 'Silva' (so a mid-word hit is not flagged), while
-- '-' + 'rowe' is in 'Smith-Rowe' and '''' + 'neil' is in 'O''Neil'. Because they are not ignored, 'oneil' does not match
-- 'O''Neil' at all.
-- The count is taken over all matches and the top @Take are cut BEFORE the club and the position are looked up, so those are
-- only looked up for the rows returned. An unknown @LeagueTeamId leaves @LeagueId NULL: the players filter is false and the
-- roster join matches nothing, so all four sets still come back, with counts of 0 and no rows.
-- Every player name is scanned (nothing indexes Name and CHARINDEX cannot seek), as dbo.SearchPlayers does. The club lookup
-- is served by IX_LeagueTeamPlayers_PlayerId, the roster by UQ_LeagueTeams_LeagueId_TeamId.
CREATE OR ALTER PROCEDURE dbo.SearchLeaguePlayersAndTeams
    @LeagueTeamId INT,
    @SearchTerm   NVARCHAR(100),
    @Take         INT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @LeagueId INT = (SELECT lt.LeagueId FROM dbo.LeagueTeams lt WHERE lt.Id = @LeagueTeamId);

    -- The matches are kept once, with the current rating, so the count and the top rows agree.
    SELECT p.Id,
           p.Name,
           CASE WHEN CHARINDEX(@SearchTerm, p.Name COLLATE Latin1_General_100_CI_AI) = 1
                  OR CHARINDEX(N' ' + @SearchTerm, p.Name COLLATE Latin1_General_100_CI_AI) > 0
                  OR CHARINDEX(N'-' + @SearchTerm, p.Name COLLATE Latin1_General_100_CI_AI) > 0
                  OR CHARINDEX(N'''' + @SearchTerm, p.Name COLLATE Latin1_General_100_CI_AI) > 0
                  OR CHARINDEX(NCHAR(8217) + @SearchTerm, p.Name COLLATE Latin1_General_100_CI_AI) > 0
                THEN 1 ELSE 0 END AS WordStart,
           cr.Rating
    INTO #PlayerMatches
    FROM dbo.Players p
        LEFT JOIN dbo.PlayerRatings cr ON cr.PlayerId = p.Id AND cr.IsCurrent = 1
    WHERE @LeagueId IS NOT NULL
      AND CHARINDEX(@SearchTerm, p.Name COLLATE Latin1_General_100_CI_AI) > 0;

    -- The best rows are cut before the club and the position are looked up. NULL ratings sort last under DESC.
    SELECT TOP (@Take) m.Id, m.Name, m.WordStart, m.Rating
    INTO #TopPlayers
    FROM #PlayerMatches m
    ORDER BY m.WordStart DESC, m.Rating DESC, m.Name, m.Id;

    SELECT COUNT(*) AS TotalCount
    FROM #PlayerMatches;

    SELECT m.Id AS PlayerId,
           m.Name,
           p.ImageUrl,
           m.Rating,
           pos.Name AS PositionName,
           pos.ShortName AS PositionShortName,
           club.LeagueTeamId,
           club.TeamName
    FROM #TopPlayers m
        INNER JOIN dbo.Players p ON p.Id = m.Id
        OUTER APPLY (
            SELECT TOP (1) lt.Id AS LeagueTeamId, t.Name AS TeamName
            FROM dbo.LeagueTeamPlayers ltp
                INNER JOIN dbo.LeagueTeams lt ON lt.Id = ltp.LeagueTeamId AND lt.LeagueId = @LeagueId
                INNER JOIN dbo.Teams t ON t.Id = lt.TeamId
            WHERE ltp.PlayerId = m.Id
            ORDER BY CASE WHEN ltp.LeagueTeamId = @LeagueTeamId THEN 0 ELSE 1 END,
                     ltp.Id
        ) club
        OUTER APPLY dbo.GetPlayerMainPosition(m.Id) mp
        LEFT JOIN dbo.Positions pos ON pos.Id = mp.PositionId
    ORDER BY m.WordStart DESC, m.Rating DESC, m.Name, m.Id;

    -- The clubs that match are flagged by the same word-start rule as the players above, so keep the two copies in step.
    SELECT lt.Id AS LeagueTeamId,
           t.Name AS TeamName,
           t.LogoUrl AS TeamLogoUrl,
           CASE WHEN CHARINDEX(@SearchTerm, t.Name COLLATE Latin1_General_100_CI_AI) = 1
                  OR CHARINDEX(N' ' + @SearchTerm, t.Name COLLATE Latin1_General_100_CI_AI) > 0
                  OR CHARINDEX(N'-' + @SearchTerm, t.Name COLLATE Latin1_General_100_CI_AI) > 0
                  OR CHARINDEX(N'''' + @SearchTerm, t.Name COLLATE Latin1_General_100_CI_AI) > 0
                  OR CHARINDEX(NCHAR(8217) + @SearchTerm, t.Name COLLATE Latin1_General_100_CI_AI) > 0
                THEN 1 ELSE 0 END AS WordStart
    INTO #TeamMatches
    FROM dbo.LeagueTeams lt
        INNER JOIN dbo.Teams t ON t.Id = lt.TeamId
    WHERE lt.LeagueId = @LeagueId
      AND CHARINDEX(@SearchTerm, t.Name COLLATE Latin1_General_100_CI_AI) > 0;

    SELECT COUNT(*) AS TotalCount
    FROM #TeamMatches;

    SELECT TOP (@Take) m.LeagueTeamId, m.TeamName, m.TeamLogoUrl
    FROM #TeamMatches m
    ORDER BY m.WordStart DESC, m.TeamName, m.LeagueTeamId;
END
