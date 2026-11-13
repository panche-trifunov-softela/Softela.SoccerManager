-- dbo.GetGameWorldPlayerProfile is renamed dbo.GetLeaguePlayerProfile (R__GetLeaguePlayerProfile.sql), because the code never says game world; the league is the thing the read is about.
-- A database that already ran the old repeatable script still holds the old procedure and nothing would remove it, since deleting an R__ file drops nothing, so this migration drops it; the R__GetGameWorldPlayerProfile.sql file is deleted in this same change.
-- It is dropped with IF EXISTS, not a bare DROP PROCEDURE, because Evolve runs every versioned (V*) script before any repeatable (R__) script, so on a brand-new database this migration runs at a point where the old procedure has never been created.

DROP PROCEDURE IF EXISTS dbo.GetGameWorldPlayerProfile;
