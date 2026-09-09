-- NVARCHAR(100) matches the application's CreateDivisionValidator MaximumLength(100).
-- The UQ_Divisions_LeagueId_Order index below already covers lookups filtered by LeagueId, so no separate index is needed for GetDivisions.
CREATE TABLE dbo.Divisions (
    Id              INT              IDENTITY(1,1)    NOT NULL,
    LeagueId        INT                               NOT NULL,
    Name            NVARCHAR(100)                     NOT NULL,
    [Order]         INT                               NOT NULL,
    TeamsPromoted   INT                               NOT NULL,
    TeamsRelegated  INT                               NOT NULL,
    TeamsInPlayoffs INT                               NOT NULL,
    CreatedAt       DATETIME2(7)                      NOT NULL,
    ModifiedAt      DATETIME2(7)                      NOT NULL,
    CreatedBy       UNIQUEIDENTIFIER                  NOT NULL,
    ModifiedBy      UNIQUEIDENTIFIER                  NOT NULL,
    CONSTRAINT PK_Divisions PRIMARY KEY CLUSTERED (Id),
    CONSTRAINT FK_Divisions_Leagues FOREIGN KEY (LeagueId) REFERENCES dbo.Leagues (Id) ON DELETE CASCADE,
    CONSTRAINT UQ_Divisions_LeagueId_Order UNIQUE (LeagueId, [Order])
);
