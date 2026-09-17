-- Only FK_Matches_Seasons cascades. Seasons and Divisions BOTH cascade from Leagues, so cascading on DivisionId as well would give Leagues two cascade paths to Matches, which SQL Server rejects outright; Divisions therefore uses the default NO ACTION.
-- Both team foreign keys use NO ACTION as well: two cascading paths from Teams into the same table are rejected for the same reason, so a team that has matches cannot be deleted until they are.
-- CK_Matches_DifferentTeams mirrors the validators' rule that the away team differs from the home team, so the two agree.
-- RefereeId deliberately carries no foreign key: dbo.Referees does not exist yet, and an FK to a missing table would make the Evolve migration fail at startup. The constraint should be added by the referee migration later.
-- Commentary is the native json type, so the database itself rejects anything that is not a JSON document; the application stores and reads it as a string and imposes no structure.
-- StartDateTime is DATETIME2(7) holding UTC, the same convention as the audit columns; it is supplied by application code, never parsed from client input.
-- The four indexes back the NO ACTION checks SQL Server runs on every parent delete and the FK-based lookups.
CREATE TABLE dbo.Matches (
    Id            INT              IDENTITY(1,1)    NOT NULL,
    SeasonId      INT                               NOT NULL,
    DivisionId    INT                               NOT NULL,
    HomeTeamId    INT                               NOT NULL,
    AwayTeamId    INT                               NOT NULL,
    RefereeId     INT                               NOT NULL,
    StartDateTime DATETIME2(7)                      NOT NULL,
    Commentary    JSON                                  NULL,
    CreatedAt     DATETIME2(7)                      NOT NULL,
    ModifiedAt    DATETIME2(7)                      NOT NULL,
    CreatedBy     UNIQUEIDENTIFIER                  NOT NULL,
    ModifiedBy    UNIQUEIDENTIFIER                  NOT NULL,
    CONSTRAINT PK_Matches PRIMARY KEY CLUSTERED (Id),
    CONSTRAINT FK_Matches_Seasons FOREIGN KEY (SeasonId) REFERENCES dbo.Seasons (Id) ON DELETE CASCADE,
    CONSTRAINT FK_Matches_Divisions FOREIGN KEY (DivisionId) REFERENCES dbo.Divisions (Id),
    CONSTRAINT FK_Matches_HomeTeams FOREIGN KEY (HomeTeamId) REFERENCES dbo.Teams (Id),
    CONSTRAINT FK_Matches_AwayTeams FOREIGN KEY (AwayTeamId) REFERENCES dbo.Teams (Id),
    CONSTRAINT CK_Matches_DifferentTeams CHECK (HomeTeamId <> AwayTeamId)
);

CREATE NONCLUSTERED INDEX IX_Matches_SeasonId ON dbo.Matches (SeasonId);
CREATE NONCLUSTERED INDEX IX_Matches_DivisionId ON dbo.Matches (DivisionId);
CREATE NONCLUSTERED INDEX IX_Matches_HomeTeamId ON dbo.Matches (HomeTeamId);
CREATE NONCLUSTERED INDEX IX_Matches_AwayTeamId ON dbo.Matches (AwayTeamId);
