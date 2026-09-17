-- Adds LogoUrl to dbo.Teams. It is an additive, nullable column so existing rows are unaffected and V1_0_0_04 stays untouched: Evolve treats an applied versioned script as immutable.
-- NVARCHAR(500) deliberately matches the application's CreateTeamValidator MaximumLength(500), so the two agree rather than the database silently truncating.
-- LogoUrl holds a URL only; no image bytes are stored here, upload is deferred.
ALTER TABLE dbo.Teams ADD LogoUrl NVARCHAR(500) NULL;
