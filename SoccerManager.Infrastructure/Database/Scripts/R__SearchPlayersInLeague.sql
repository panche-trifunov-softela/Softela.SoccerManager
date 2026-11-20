-- One page of the players matching every given filter, seen from the league of @LeagueTeamId, behind
-- GET /api/league-teams/{id}/league-players, as two result sets: the match count first, then the page. It does not check
-- access: the query handler does that first, through IClubAccess.
-- The filters, sort keys, defaults and the matching of the name are those of dbo.SearchPlayers (CHARINDEX under an accent-
-- and case-insensitive collation, so the LIKE wildcards and the bracket mean nothing in the term; the caller turns a blank
-- term into NULL; the NULL defaults give rating, descending, page 1 of 25). @PositionId matches only the player's main
-- position (dbo.GetPlayerMainPosition), and a player with none gets NULL names and position, which the Dapper model leaves
-- as empty strings and 0.
-- Players are every dbo.Players row. A player's club is the registration at a club of @LeagueTeamId's league, picked as
-- dbo.SearchLeaguePlayersAndTeams picks it: @LeagueTeamId's own first, else the lowest dbo.LeagueTeamPlayers.Id (V1_0_0_37
-- dropped the unique key, so a player can be registered twice; the TOP (1) shows such a player once). A player with no
-- registration at any club of the league is a free agent, with NULL LeagueTeamId and TeamName, and the @TeamId filter,
-- which matches the club of that registration, leaves him out.
-- Value and Wage are the registration's TransfermarketValue and ContractSalaryPerWeek (BIGINT, cast to DECIMAL(18,2)), as
-- dbo.GetLeaguePlayerProfile shows them, and for a free agent dbo.Players.[Value] and Wage, which is what his panel shows;
-- they are never NULL, and the value and wage sorts and the count use these figures.
-- The rating is the player's current dbo.PlayerRatings row (at most one by UX_PlayerRatings_PlayerId_Current), a LEFT JOIN,
-- so an unrated player is listed, with a NULL Rating. The rating range leaves him out; sorting by rating puts him last
-- whichever the direction.
-- An unknown @LeagueTeamId leaves @LeagueId NULL, which makes the matches empty, so both sets still come back, with a count
-- of 0 and no rows. The count is a separate set, so a page past the end keeps it.
-- Every player name is scanned (nothing indexes Name and CHARINDEX cannot seek), and the club is looked up for every
-- matching player, served by IX_LeagueTeamPlayers_PlayerId.
CREATE OR ALTER PROCEDURE dbo.SearchPlayersInLeague
    @LeagueTeamId   INT,
    @SearchTerm     NVARCHAR(100) = NULL,
    @PositionId     INT           = NULL,
    @TeamId         INT           = NULL,
    @NationalTeamId INT           = NULL,
    @MinRating      INT           = NULL,
    @MaxRating      INT           = NULL,
    @SortBy         TINYINT       = NULL,
    @SortDescending BIT           = NULL,
    @Page           INT           = NULL,
    @PageSize       INT           = NULL
AS
BEGIN
    SET NOCOUNT ON;

    -- A NULL OFFSET or FETCH is an error, so NULL means the default.
    SET @SortBy = COALESCE(@SortBy, 2);
    SET @SortDescending = COALESCE(@SortDescending, 1);
    SET @Page = COALESCE(@Page, 1);
    SET @PageSize = COALESCE(@PageSize, 25);

    DECLARE @LeagueId INT = (SELECT lt.LeagueId FROM dbo.LeagueTeams lt WHERE lt.Id = @LeagueTeamId);

    -- The matches are kept once, with the current rating, the club and the figures, so the count and the page agree.
    SELECT p.Id,
           mp.PositionId,
           cr.Rating,
           club.LeagueTeamId,
           club.TeamName,
           COALESCE(CAST(club.TransfermarketValue AS DECIMAL(18,2)), p.[Value]) AS [Value],
           COALESCE(CAST(club.ContractSalaryPerWeek AS DECIMAL(18,2)), p.Wage) AS Wage
    INTO #Matches
    FROM dbo.Players p
        LEFT JOIN dbo.PlayerRatings cr ON cr.PlayerId = p.Id AND cr.IsCurrent = 1
        OUTER APPLY dbo.GetPlayerMainPosition(p.Id) mp
        OUTER APPLY (
            SELECT TOP (1) lt.Id AS LeagueTeamId,
                           lt.TeamId,
                           t.Name AS TeamName,
                           ltp.TransfermarketValue,
                           ltp.ContractSalaryPerWeek
            FROM dbo.LeagueTeamPlayers ltp
                INNER JOIN dbo.LeagueTeams lt ON lt.Id = ltp.LeagueTeamId AND lt.LeagueId = @LeagueId
                INNER JOIN dbo.Teams t ON t.Id = lt.TeamId
            WHERE ltp.PlayerId = p.Id
            ORDER BY CASE WHEN ltp.LeagueTeamId = @LeagueTeamId THEN 0 ELSE 1 END,
                     ltp.Id
        ) club
    WHERE @LeagueId IS NOT NULL
      AND (@SearchTerm IS NULL OR CHARINDEX(@SearchTerm, p.Name COLLATE Latin1_General_100_CI_AI) > 0)
      AND (@PositionId IS NULL OR mp.PositionId = @PositionId)
      AND (@TeamId IS NULL OR club.TeamId = @TeamId)
      AND (@NationalTeamId IS NULL OR p.NationalTeamId = @NationalTeamId)
      AND (@MinRating IS NULL OR cr.Rating >= @MinRating)
      AND (@MaxRating IS NULL OR cr.Rating <= @MaxRating);

    -- Counted on its own: on a page past the end, a COUNT(*) OVER () would return no row at all.
    SELECT COUNT(*) AS TotalCount
    FROM #Matches;

    -- @SortBy: 1 name, 2 rating, 3 age (ascending is youngest first, so date of birth runs the other way),
    -- 4 value, 5 wage. An unrated player goes last when sorting by rating, in both directions. Ties fall back to Id in the
    -- same direction, so reversing it reverses the whole list.
    SELECT m.Id AS PlayerId,
           p.Name,
           p.DateOfBirth,
           m.Rating,
           m.[Value],
           m.Wage,
           p.ImageUrl,
           m.LeagueTeamId,
           m.TeamName,
           p.NationalTeamId,
           nt.Name  AS NationalTeamName,
           m.PositionId,
           pos.Name AS PositionName,
           pos.ShortName AS PositionShortName
    FROM #Matches m
        INNER JOIN dbo.Players p ON p.Id = m.Id
        LEFT JOIN dbo.NationalTeams nt ON nt.Id = p.NationalTeamId
        LEFT JOIN dbo.Positions pos ON pos.Id = m.PositionId
    ORDER BY CASE WHEN @SortBy = 2 AND m.Rating IS NULL THEN 1 ELSE 0 END,
             CASE WHEN @SortBy = 1 AND @SortDescending = 0 THEN p.Name END,
             CASE WHEN @SortBy = 1 AND @SortDescending = 1 THEN p.Name END DESC,
             CASE WHEN @SortBy = 2 AND @SortDescending = 0 THEN m.Rating END,
             CASE WHEN @SortBy = 2 AND @SortDescending = 1 THEN m.Rating END DESC,
             CASE WHEN @SortBy = 3 AND @SortDescending = 0 THEN p.DateOfBirth END DESC,
             CASE WHEN @SortBy = 3 AND @SortDescending = 1 THEN p.DateOfBirth END,
             CASE WHEN @SortBy = 4 AND @SortDescending = 0 THEN m.[Value] END,
             CASE WHEN @SortBy = 4 AND @SortDescending = 1 THEN m.[Value] END DESC,
             CASE WHEN @SortBy = 5 AND @SortDescending = 0 THEN m.Wage END,
             CASE WHEN @SortBy = 5 AND @SortDescending = 1 THEN m.Wage END DESC,
             CASE WHEN @SortDescending = 0 THEN m.Id END,
             CASE WHEN @SortDescending = 1 THEN m.Id END DESC
    -- BIGINT, so a huge @Page gives an empty page instead of an arithmetic overflow.
    OFFSET (CAST(@Page AS BIGINT) - 1) * @PageSize ROWS
    FETCH NEXT @PageSize ROWS ONLY;
END
