-- MatchId and TeamId cascade: the only cascading path into Matches is Leagues -> Seasons -> Matches (its Divisions key is NO ACTION), and the only foreign key Teams carries is FK_Teams_Stadiums (SET NULL) with Stadiums reaching nothing else, so each parent reaches this table by a single path and SQL Server accepts both cascades.
-- FormationPositionId and PlayerPositionId are both NO ACTION, deliberately: FormationPositions and PlayerPositions each cascade from Positions, so cascading from both here would give Positions two cascade paths into this table, which SQL Server rejects outright. Beyond that, a row here is the record of who played where, so deleting a formation slot or a player's position rating must not silently erase it; the rows referencing it have to go first.
-- UQ_MatchFormationPlayerPositions_MatchId_TeamId_FormationPositionId puts one player in each slot per team per match; it leads on MatchId, so it also backs the WHERE MatchId = @MatchId lookup in GetMatchFormationPlayerPositions and the cascade from Matches.
-- IX_MatchFormationPlayerPositions_TeamId backs the cascade from Teams; the FormationPositionId and PlayerPositionId indexes back the NO ACTION checks SQL Server runs when a slot or a rating is deleted.
-- ConditionOnMatch and QualityAtPositionOnMatch are snapshots the application copies from LeagueTeamPlayers.Condition and PlayerPositions.Quality at write time, so they carry the same 1-100 CHECK as their sources. Neither is ever accepted from a client, so the validators carry no rule for them and the database check is the only guard.
-- IsSuspended is likewise copied from LeagueTeamPlayers at write time. No CHECK, since BIT admits only 0 and 1.
CREATE TABLE dbo.MatchFormationPlayerPositions (
    Id                       INT              IDENTITY(1,1)    NOT NULL,
    MatchId                  INT                               NOT NULL,
    TeamId                   INT                               NOT NULL,
    FormationPositionId      INT                               NOT NULL,
    PlayerPositionId         INT                               NOT NULL,
    ConditionOnMatch         INT                               NOT NULL,
    QualityAtPositionOnMatch INT                               NOT NULL,
    IsSuspended              BIT                               NOT NULL,
    CreatedAt                DATETIME2(7)                      NOT NULL,
    ModifiedAt               DATETIME2(7)                      NOT NULL,
    CreatedBy                UNIQUEIDENTIFIER                  NOT NULL,
    ModifiedBy               UNIQUEIDENTIFIER                  NOT NULL,
    CONSTRAINT PK_MatchFormationPlayerPositions PRIMARY KEY CLUSTERED (Id),
    CONSTRAINT FK_MatchFormationPlayerPositions_Matches FOREIGN KEY (MatchId) REFERENCES dbo.Matches (Id) ON DELETE CASCADE,
    CONSTRAINT FK_MatchFormationPlayerPositions_Teams FOREIGN KEY (TeamId) REFERENCES dbo.Teams (Id) ON DELETE CASCADE,
    CONSTRAINT FK_MatchFormationPlayerPositions_FormationPositions FOREIGN KEY (FormationPositionId) REFERENCES dbo.FormationPositions (Id),
    CONSTRAINT FK_MatchFormationPlayerPositions_PlayerPositions FOREIGN KEY (PlayerPositionId) REFERENCES dbo.PlayerPositions (Id),
    CONSTRAINT UQ_MatchFormationPlayerPositions_MatchId_TeamId_FormationPositionId UNIQUE (MatchId, TeamId, FormationPositionId),
    CONSTRAINT CK_MatchFormationPlayerPositions_ConditionOnMatch CHECK (ConditionOnMatch BETWEEN 1 AND 100),
    CONSTRAINT CK_MatchFormationPlayerPositions_QualityAtPositionOnMatch CHECK (QualityAtPositionOnMatch BETWEEN 1 AND 100)
);

CREATE NONCLUSTERED INDEX IX_MatchFormationPlayerPositions_TeamId ON dbo.MatchFormationPlayerPositions (TeamId);
CREATE NONCLUSTERED INDEX IX_MatchFormationPlayerPositions_FormationPositionId ON dbo.MatchFormationPlayerPositions (FormationPositionId);
CREATE NONCLUSTERED INDEX IX_MatchFormationPlayerPositions_PlayerPositionId ON dbo.MatchFormationPlayerPositions (PlayerPositionId);
