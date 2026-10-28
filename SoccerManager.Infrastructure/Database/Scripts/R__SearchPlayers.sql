-- One page of the players matching every given filter, with the match count first as its own result set.
-- CHARINDEX matches the term literally under an accent- and case-insensitive collation, so a term typed
-- without accents still finds an accented name whatever the database's own collation; the caller turns a
-- blank term into NULL. The NULL defaults (rating, descending, page 1 of 25) let a caller passing no
-- parameters get the first page.
-- A player's rating is its current dbo.PlayerRatings row. MyVote is @UserId's vote on that row (1 up,
-- 0 down) and MyVoteId that vote's Id, both NULL when they have cast none on it: a vote on an earlier
-- rating does not count, even one with the same value. UQ_Managers_UserId and
-- UQ_ManagerPlayerRatingVotes_ManagerId_PlayerRatingId allow at most one such vote, so the OUTER APPLY
-- never adds a row. A NULL @UserId (an older API build on the shared database passes none) gives NULL
-- throughout.
CREATE OR ALTER PROCEDURE dbo.SearchPlayers
    @SearchTerm     NVARCHAR(100) = NULL,
    @PositionId     INT           = NULL,
    @TeamId         INT           = NULL,
    @NationalTeamId INT           = NULL,
    @MinRating      INT           = NULL,
    @MaxRating      INT           = NULL,
    @SortBy         TINYINT       = NULL,
    @SortDescending BIT           = NULL,
    @Page           INT           = NULL,
    @PageSize       INT           = NULL,
    @UserId         UNIQUEIDENTIFIER = NULL
AS
BEGIN
    SET NOCOUNT ON;

    -- A NULL OFFSET or FETCH is an error, so NULL means the default.
    SET @SortBy = COALESCE(@SortBy, 2);
    SET @SortDescending = COALESCE(@SortDescending, 1);
    SET @Page = COALESCE(@Page, 1);
    SET @PageSize = COALESCE(@PageSize, 25);

    -- @PositionId matches only the player's main position (dbo.GetPlayerMainPosition). The matches are kept once,
    -- with the current rating, so the count and the page agree.
    SELECT p.Id, mp.PositionId, cr.Id AS PlayerRatingId, cr.Rating
    INTO #Matches
    FROM dbo.Players p
        INNER JOIN dbo.PlayerRatings cr ON cr.PlayerId = p.Id AND cr.IsCurrent = 1
        OUTER APPLY dbo.GetPlayerMainPosition(p.Id) mp
    WHERE (@SearchTerm IS NULL OR CHARINDEX(@SearchTerm, p.Name COLLATE Latin1_General_100_CI_AI) > 0)
      AND (@PositionId IS NULL OR mp.PositionId = @PositionId)
      AND (@TeamId IS NULL OR p.TeamId = @TeamId)
      AND (@NationalTeamId IS NULL OR p.NationalTeamId = @NationalTeamId)
      AND (@MinRating IS NULL OR cr.Rating >= @MinRating)
      AND (@MaxRating IS NULL OR cr.Rating <= @MaxRating);

    -- Counted on its own: on a page past the end, a COUNT(*) OVER () would return no row at all.
    SELECT COUNT(*) AS TotalCount
    FROM #Matches;

    -- @SortBy: 1 name, 2 rating, 3 age (ascending is youngest first, so date of birth runs the other way),
    -- 4 value, 5 wage. Ties fall back to Id in the same direction, so reversing it reverses the whole list.
    SELECT p.Id,
           p.Name,
           p.DateOfBirth,
           m.Rating,
           p.[Value],
           p.Wage,
           p.ImageUrl,
           p.TeamId,
           t.Name   AS TeamName,
           p.NationalTeamId,
           nt.Name  AS NationalTeamName,
           m.PositionId,
           pos.Name AS PositionName,
           pos.ShortName AS PositionShortName,
           mv.UpOrDown AS MyVote,
           mv.Id       AS MyVoteId
    FROM #Matches m
        INNER JOIN dbo.Players p ON p.Id = m.Id
        LEFT JOIN dbo.Teams t ON t.Id = p.TeamId
        LEFT JOIN dbo.NationalTeams nt ON nt.Id = p.NationalTeamId
        LEFT JOIN dbo.Positions pos ON pos.Id = m.PositionId
        OUTER APPLY (
            SELECT v.Id, v.UpOrDown
            FROM dbo.ManagerPlayerRatingVotes v
                INNER JOIN dbo.Managers mgr ON mgr.Id = v.ManagerId
            WHERE v.PlayerRatingId = m.PlayerRatingId
              AND mgr.UserId = @UserId
        ) mv
    ORDER BY CASE WHEN @SortBy = 1 AND @SortDescending = 0 THEN p.Name END,
             CASE WHEN @SortBy = 1 AND @SortDescending = 1 THEN p.Name END DESC,
             CASE WHEN @SortBy = 2 AND @SortDescending = 0 THEN m.Rating END,
             CASE WHEN @SortBy = 2 AND @SortDescending = 1 THEN m.Rating END DESC,
             CASE WHEN @SortBy = 3 AND @SortDescending = 0 THEN p.DateOfBirth END DESC,
             CASE WHEN @SortBy = 3 AND @SortDescending = 1 THEN p.DateOfBirth END,
             CASE WHEN @SortBy = 4 AND @SortDescending = 0 THEN p.[Value] END,
             CASE WHEN @SortBy = 4 AND @SortDescending = 1 THEN p.[Value] END DESC,
             CASE WHEN @SortBy = 5 AND @SortDescending = 0 THEN p.Wage END,
             CASE WHEN @SortBy = 5 AND @SortDescending = 1 THEN p.Wage END DESC,
             CASE WHEN @SortDescending = 0 THEN p.Id END,
             CASE WHEN @SortDescending = 1 THEN p.Id END DESC
    -- BIGINT, so a huge @Page gives an empty page instead of an arithmetic overflow.
    OFFSET (CAST(@Page AS BIGINT) - 1) * @PageSize ROWS
    FETCH NEXT @PageSize ROWS ONLY;
END
