-- A player's main position: their highest-Quality dbo.PlayerPositions row, the first entered (lowest Id) winning a tie.
-- An inline table-valued function, so SQL Server expands it into the calling query like a view; callers OUTER APPLY it, and a
-- player with no position gets no row. No SCHEMABINDING, so a later migration can still alter dbo.PlayerPositions.
CREATE OR ALTER FUNCTION dbo.GetPlayerMainPosition
(
    @PlayerId INT
)
RETURNS TABLE
AS
RETURN
(
    SELECT TOP (1) pp.PositionId
    FROM dbo.PlayerPositions pp
    WHERE pp.PlayerId = @PlayerId
    ORDER BY pp.Quality DESC, pp.Id
);
