-- V1_0_0_06 is already applied and is never edited; Evolve rejects a changed checksum.
-- ShortName is a position's abbreviation (GK, CB, ...) and CatalogOrder its place in the importer's catalog (SoccerManager.Importer/Transform/PositionCatalog.cs), which orders the positions of one line.
-- Both are filled for the 13 catalog positions by Name, then made NOT NULL, because ALTER TABLE ... ADD of a NOT NULL column fails on a table that already has rows.
-- Every existing row is one of the 13 and a fresh database has no rows yet; a row with any other Name leaves a NULL, the ALTER COLUMN fails and Evolve rolls the whole script back.
-- NVARCHAR(10) deliberately matches the validators' MaximumLength(10), so the two agree rather than the database silently truncating.
-- CK_Positions_CatalogOrder mirrors the validators' GreaterThan(0).
ALTER TABLE dbo.Positions ADD ShortName NVARCHAR(10) NULL, CatalogOrder TINYINT NULL;
GO

UPDATE p
SET p.ShortName = c.ShortName,
    p.CatalogOrder = c.CatalogOrder
FROM dbo.Positions p
    INNER JOIN (VALUES
        (N'Goalkeeper', N'GK', 1),
        (N'Centre-Back', N'CB', 2),
        (N'Left-Back', N'LB', 3),
        (N'Right-Back', N'RB', 4),
        (N'Defensive Midfield', N'DM', 5),
        (N'Central Midfield', N'CM', 6),
        (N'Attacking Midfield', N'AM', 7),
        (N'Left Midfield', N'LM', 8),
        (N'Right Midfield', N'RM', 9),
        (N'Left Winger', N'LW', 10),
        (N'Right Winger', N'RW', 11),
        (N'Second Striker', N'SS', 12),
        (N'Centre-Forward', N'CF', 13)
    ) c (Name, ShortName, CatalogOrder) ON c.Name = p.Name;

ALTER TABLE dbo.Positions ALTER COLUMN ShortName NVARCHAR(10) NOT NULL;

ALTER TABLE dbo.Positions ALTER COLUMN CatalogOrder TINYINT NOT NULL;

ALTER TABLE dbo.Positions ADD CONSTRAINT CK_Positions_CatalogOrder CHECK (CatalogOrder >= 1);
