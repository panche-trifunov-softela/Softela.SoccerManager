-- dbo.MatchFormationPlayerPositionSubstitutions holds the lineup of each substitution in dbo.MatchTeamTacticSubstitutions: every slot of the substitution's own formation, under the same match, club, minute and situation, with the same snapshot columns as dbo.MatchFormationPlayerPositions.
-- FK_MatchFormationPlayerPositionSubstitutions_Matches cascades and FK_MatchFormationPlayerPositionSubstitutions_LeagueTeams does not, for the reasons V1_0_0_43 gives: Leagues already cascade into this table through Seasons and Matches, and a second path through LeagueTeams would be rejected as multiple cascade paths.
-- FormationPositionId and PlayerPositionId are NO ACTION for the reasons V1_0_0_25 gives: FormationPositions and PlayerPositions each cascade from Positions, so cascading from both would give Positions multiple cascade paths into this table, and a substitution's rows have to go before the slot or rating they point at.
-- UQ_MatchFormationPlayerPositionSubstitutions_MatchId_LeagueTeamId_MinuteAt_Situation_FormationPositionId puts one player in each slot of a substitution. It leads on MatchId and LeagueTeamId, so it also backs the per-club reads and deletes and the cascade from Matches.
-- CK_MatchFormationPlayerPositionSubstitutions_MinuteAt matches CK_MatchTeamTacticSubstitutions_MinuteAt. ConditionOnMatch and QualityAtPositionOnMatch carry the 1-100 CHECKs of V1_0_0_25, since they are the same snapshots; IsSuspended has no CHECK, since BIT admits only 0 and 1; Situation has no CHECK, like every other enum column.
-- IX_MatchFormationPlayerPositionSubstitutions_LeagueTeamId backs the foreign key and the roster-removal check; the FormationPositionId and PlayerPositionId indexes back the NO ACTION checks SQL Server runs when a slot or a rating is deleted.
CREATE TABLE dbo.MatchFormationPlayerPositionSubstitutions (
    Id                       INT              IDENTITY(1,1)    NOT NULL,
    MatchId                  INT                               NOT NULL,
    LeagueTeamId             INT                               NOT NULL,
    MinuteAt                 INT                               NOT NULL,
    Situation                TINYINT                           NOT NULL,
    FormationPositionId      INT                               NOT NULL,
    PlayerPositionId         INT                               NOT NULL,
    ConditionOnMatch         INT                               NOT NULL,
    QualityAtPositionOnMatch INT                               NOT NULL,
    IsSuspended              BIT                               NOT NULL,
    CreatedAt                DATETIME2(7)                      NOT NULL,
    ModifiedAt               DATETIME2(7)                      NOT NULL,
    CreatedBy                UNIQUEIDENTIFIER                  NOT NULL,
    ModifiedBy               UNIQUEIDENTIFIER                  NOT NULL,
    CONSTRAINT PK_MatchFormationPlayerPositionSubstitutions PRIMARY KEY CLUSTERED (Id),
    CONSTRAINT FK_MatchFormationPlayerPositionSubstitutions_Matches FOREIGN KEY (MatchId) REFERENCES dbo.Matches (Id) ON DELETE CASCADE,
    CONSTRAINT FK_MatchFormationPlayerPositionSubstitutions_LeagueTeams FOREIGN KEY (LeagueTeamId) REFERENCES dbo.LeagueTeams (Id),
    CONSTRAINT FK_MatchFormationPlayerPositionSubstitutions_FormationPositions FOREIGN KEY (FormationPositionId) REFERENCES dbo.FormationPositions (Id),
    CONSTRAINT FK_MatchFormationPlayerPositionSubstitutions_PlayerPositions FOREIGN KEY (PlayerPositionId) REFERENCES dbo.PlayerPositions (Id),
    CONSTRAINT UQ_MatchFormationPlayerPositionSubstitutions_MatchId_LeagueTeamId_MinuteAt_Situation_FormationPositionId UNIQUE (MatchId, LeagueTeamId, MinuteAt, Situation, FormationPositionId),
    CONSTRAINT CK_MatchFormationPlayerPositionSubstitutions_MinuteAt CHECK (MinuteAt IN (45, 60, 75)),
    CONSTRAINT CK_MatchFormationPlayerPositionSubstitutions_ConditionOnMatch CHECK (ConditionOnMatch BETWEEN 1 AND 100),
    CONSTRAINT CK_MatchFormationPlayerPositionSubstitutions_QualityAtPositionOnMatch CHECK (QualityAtPositionOnMatch BETWEEN 1 AND 100)
);

CREATE NONCLUSTERED INDEX IX_MatchFormationPlayerPositionSubstitutions_LeagueTeamId ON dbo.MatchFormationPlayerPositionSubstitutions (LeagueTeamId);
CREATE NONCLUSTERED INDEX IX_MatchFormationPlayerPositionSubstitutions_FormationPositionId ON dbo.MatchFormationPlayerPositionSubstitutions (FormationPositionId);
CREATE NONCLUSTERED INDEX IX_MatchFormationPlayerPositionSubstitutions_PlayerPositionId ON dbo.MatchFormationPlayerPositionSubstitutions (PlayerPositionId);
