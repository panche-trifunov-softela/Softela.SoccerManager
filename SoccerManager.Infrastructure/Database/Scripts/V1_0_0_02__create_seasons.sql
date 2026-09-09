-- The UQ_Seasons_LeagueId_SeasonNumber index below already covers lookups filtered by LeagueId, so no separate index is needed for GetSeasons.
CREATE TABLE dbo.Seasons (
    Id           INT              IDENTITY(1,1)    NOT NULL,
    LeagueId     INT                               NOT NULL,
    SeasonNumber INT                               NOT NULL,
    CreatedAt    DATETIME2(7)                      NOT NULL,
    ModifiedAt   DATETIME2(7)                      NOT NULL,
    CreatedBy    UNIQUEIDENTIFIER                  NOT NULL,
    ModifiedBy   UNIQUEIDENTIFIER                  NOT NULL,
    CONSTRAINT PK_Seasons PRIMARY KEY CLUSTERED (Id),
    CONSTRAINT FK_Seasons_Leagues FOREIGN KEY (LeagueId) REFERENCES dbo.Leagues (Id) ON DELETE CASCADE,
    CONSTRAINT UQ_Seasons_LeagueId_SeasonNumber UNIQUE (LeagueId, SeasonNumber)
);
