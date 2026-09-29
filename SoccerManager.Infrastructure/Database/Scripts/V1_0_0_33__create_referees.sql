-- NVARCHAR(100) deliberately matches the application's CreateRefereeValidator MaximumLength(100), and NVARCHAR(500) does the same for ImageUrl, so the two agree rather than the database silently truncating.
-- Name is deliberately not unique: real referees can share a name, and telling them apart is left for later.
-- ImageUrl holds a URL only; no image bytes are stored here, upload is deferred.
-- Tolerance is TINYINT holding the Tolerance enum value, where 1 is Low, 2 is Balanced and 3 is High. Zero is deliberately unused, so a value that was never set is rejected rather than stored as Low, and like every other enum column it has no CHECK constraint. DF_Referees_Tolerance makes a row created outside the insert procedure Balanced, the same default the requests apply; the procedure itself always passes the value.
-- FK_Matches_Referees is the constraint V1_0_0_13 deferred until this table existed. Matches.RefereeId becomes nullable first, since SET NULL needs it and a match may have no referee, and the UPDATE then clears every existing value, because none ever referred to a real referee and adding the foreign key with the default WITH CHECK would otherwise fail on such a row.
-- FK_Matches_Referees uses ON DELETE SET NULL, so deleting a referee un-assigns them from their matches. SET NULL touches only Matches rows, so it never reaches the tables that cascade from Matches, and dbo.Referees has no parents, so this is its only path into Matches.
-- IX_Matches_RefereeId backs that SET NULL, since every referee deletion has to find its matches.
CREATE TABLE dbo.Referees (
    Id         INT              IDENTITY(1,1)    NOT NULL,
    Name       NVARCHAR(100)                     NOT NULL,
    ImageUrl   NVARCHAR(500)                         NULL,
    Tolerance  TINYINT                           NOT NULL CONSTRAINT DF_Referees_Tolerance DEFAULT 2,
    CreatedAt  DATETIME2(7)                      NOT NULL,
    ModifiedAt DATETIME2(7)                      NOT NULL,
    CreatedBy  UNIQUEIDENTIFIER                  NOT NULL,
    ModifiedBy UNIQUEIDENTIFIER                  NOT NULL,
    CONSTRAINT PK_Referees PRIMARY KEY CLUSTERED (Id)
);

ALTER TABLE dbo.Matches ALTER COLUMN RefereeId INT NULL;

UPDATE dbo.Matches SET RefereeId = NULL WHERE RefereeId IS NOT NULL;

ALTER TABLE dbo.Matches
    ADD CONSTRAINT FK_Matches_Referees FOREIGN KEY (RefereeId) REFERENCES dbo.Referees (Id) ON DELETE SET NULL;

CREATE NONCLUSTERED INDEX IX_Matches_RefereeId ON dbo.Matches (RefereeId);
