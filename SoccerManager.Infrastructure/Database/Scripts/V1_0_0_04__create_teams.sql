-- NVARCHAR(100) deliberately matches the application's CreateTeamValidator MaximumLength(100), so the two agree rather than the database silently truncating.
-- StadiumId is deliberately nullable and carries no foreign key: dbo.Stadiums does not exist yet, and an FK to a missing table would make the Evolve migration fail at startup. The constraint should be added by the stadium migration later.
-- FinancialState is TINYINT holding the FinancialState enum value, where 1 is VeryPoor and 5 is VeryRich. Zero is deliberately unused, so a value that was never set is rejected rather than stored as VeryPoor.
-- JerseyUrl holds a URL only; no image bytes are stored here, upload is deferred.
CREATE TABLE dbo.Teams (
    Id             INT              IDENTITY(1,1)    NOT NULL,
    Name           NVARCHAR(100)                     NOT NULL,
    StadiumId      INT                                   NULL,
    FinancialState TINYINT                           NOT NULL,
    JerseyUrl      NVARCHAR(500)                         NULL,
    CreatedAt      DATETIME2(7)                      NOT NULL,
    ModifiedAt     DATETIME2(7)                      NOT NULL,
    CreatedBy      UNIQUEIDENTIFIER                  NOT NULL,
    ModifiedBy     UNIQUEIDENTIFIER                  NOT NULL,
    CONSTRAINT PK_Teams PRIMARY KEY CLUSTERED (Id)
);
