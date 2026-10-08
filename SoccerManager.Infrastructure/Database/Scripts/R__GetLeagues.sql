-- Leagues in which @UserId does not currently manage a club, newest first (Id breaks ties).
-- CHARINDEX matches the term literally (%, _ and [ need no escaping) and follows the
-- database's case-insensitive collation; the caller turns a blank term into NULL.
-- The NULL defaults let a caller passing no parameters (an older API build on the shared
-- database) still get every league.
CREATE OR ALTER PROCEDURE dbo.GetLeagues
    @UserId     UNIQUEIDENTIFIER = NULL,
    @SearchTerm NVARCHAR(100)    = NULL,
    @MaxCount   INT              = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP (COALESCE(@MaxCount, 2147483647)) l.Id, l.Name, l.CreatedAt, l.ModifiedAt, l.CreatedBy, l.ModifiedBy
    FROM dbo.Leagues l
    WHERE (@SearchTerm IS NULL OR CHARINDEX(@SearchTerm, l.Name) > 0)
      AND NOT EXISTS (
          SELECT 1
          FROM dbo.LeagueTeamManagers ltm
              INNER JOIN dbo.Managers m ON m.Id = ltm.ManagerId
          WHERE ltm.LeagueId = l.Id
            AND ltm.IsCurrent = 1
            AND m.UserId = @UserId
      )
    ORDER BY l.CreatedAt DESC, l.Id DESC;
END
