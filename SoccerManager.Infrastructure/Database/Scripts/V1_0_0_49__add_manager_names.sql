-- V1_0_0_11 is already applied and is never edited; Evolve rejects a changed checksum.
-- FirstName and LastName are copies of the manager's Keycloak given_name and family_name claims. Only
-- dbo.UpdateManagerName writes them, on POST /api/managers/me, which the UI calls on every page load, so Keycloak stays
-- the source of truth and a renamed user's copy catches up on their next visit. NVARCHAR(255) matches Keycloak's
-- default user profile, which caps firstName and lastName at 255 characters; the API trims and caps them before
-- writing too.
-- Both are nullable: an existing manager has no name until their next visit, so nothing is backfilled.
-- FullName is the one display form of the name: 'First Last', only the known part when one is missing, NULL when
-- neither is known. Every query that shows a manager's name to a caller selects it, so the format cannot drift between
-- endpoints. It is computed and not persisted, so nothing has to keep it in step. It is added in its own batch because
-- a computed column can only reference columns that already exist.
ALTER TABLE dbo.Managers ADD FirstName NVARCHAR(255) NULL, LastName NVARCHAR(255) NULL;
GO

ALTER TABLE dbo.Managers ADD FullName AS NULLIF(LTRIM(RTRIM(CONCAT(FirstName, N' ', LastName))), N'');
