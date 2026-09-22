-- NVARCHAR(100) deliberately matches the application's CreateCompetitionValidator MaximumLength(100), so the two agree rather than the database silently truncating; NVARCHAR(500) does the same for LogoUrl.
-- FK_Competitions_Leagues cascades: Competitions is a leaf that nothing references and carries no other foreign key, so Leagues reaches it by exactly one path and SQL Server accepts the cascade, the same as LeagueTeamPlayers.
-- IX_Competitions_LeagueId exists because, unlike Seasons, Divisions, LeagueTeamManagers and LeagueTeamPlayers, this table has no composite UNIQUE constraint leading on LeagueId to absorb the need, so the index is what backs the cascade from Leagues and the WHERE LeagueId = @LeagueId lookup in GetCompetitions, the same as IX_LeagueTeamPlayers_PlayerId.
-- The name is deliberately not unique: a league can run several competitions and nothing makes the name a business key, so there is no UQ constraint.
-- Format is TINYINT holding the CompetitionFormat enum value, where 1 is KnockoutOnly and 3 is GroupAndKnockout. Zero is deliberately unused, so a value that was never set is rejected rather than stored as KnockoutOnly.
-- MaxAgeAllowed is nullable, where NULL means no age limit; CK_Competitions_MaxAgeAllowed mirrors the validators' InclusiveBetween(16, 23) and uses the IS NULL OR form so the check passes on NULL, the same as CK_LeagueTeamManagers_EndDateAfterStart.
-- IsDomestic needs no CHECK, since BIT admits only 0 and 1.
-- LogoUrl holds a URL only; no image bytes are stored here.
CREATE TABLE dbo.Competitions (
    Id            INT              IDENTITY(1,1)    NOT NULL,
    LeagueId      INT                               NOT NULL,
    Name          NVARCHAR(100)                     NOT NULL,
    LogoUrl       NVARCHAR(500)                         NULL,
    IsDomestic    BIT                               NOT NULL,
    Format        TINYINT                           NOT NULL,
    MaxAgeAllowed INT                                   NULL,
    CreatedAt     DATETIME2(7)                      NOT NULL,
    ModifiedAt    DATETIME2(7)                      NOT NULL,
    CreatedBy     UNIQUEIDENTIFIER                  NOT NULL,
    ModifiedBy    UNIQUEIDENTIFIER                  NOT NULL,
    CONSTRAINT PK_Competitions PRIMARY KEY CLUSTERED (Id),
    CONSTRAINT FK_Competitions_Leagues FOREIGN KEY (LeagueId) REFERENCES dbo.Leagues (Id) ON DELETE CASCADE,
    CONSTRAINT CK_Competitions_MaxAgeAllowed CHECK (MaxAgeAllowed IS NULL OR MaxAgeAllowed BETWEEN 16 AND 23)
);

CREATE NONCLUSTERED INDEX IX_Competitions_LeagueId ON dbo.Competitions (LeagueId);
