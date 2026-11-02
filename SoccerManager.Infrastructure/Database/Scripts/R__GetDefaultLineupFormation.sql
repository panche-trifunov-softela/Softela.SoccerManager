-- The formation an auto pick uses when nothing says which: the one named @PreferredName if it has all eleven starter slots,
-- otherwise the first formation by name that does. Returns no row when no formation is complete.
CREATE OR ALTER PROCEDURE dbo.GetDefaultLineupFormation
    @PreferredName NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP (1) f.Id, f.Name, f.CreatedAt, f.ModifiedAt, f.CreatedBy, f.ModifiedBy
    FROM dbo.Formations f
    WHERE (SELECT COUNT(DISTINCT fp.SlotNumber)
           FROM dbo.FormationPositions fp
           WHERE fp.FormationId = f.Id AND fp.SlotNumber BETWEEN 1 AND 11) = 11
    ORDER BY CASE WHEN f.Name = @PreferredName THEN 0 ELSE 1 END, f.Name, f.Id;
END
