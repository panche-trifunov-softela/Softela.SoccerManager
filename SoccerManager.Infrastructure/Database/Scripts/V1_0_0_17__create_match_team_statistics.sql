-- Both foreign keys cascade: the only foreign key Teams carries is FK_Teams_Stadiums (SET NULL) and Stadiums reaches nothing else, while the only cascading path into Matches is Leagues -> Seasons -> Matches (its Divisions key is NO ACTION), so each parent reaches this table by a single path and SQL Server accepts both cascades.
-- UQ_MatchTeamStatistics_MatchId_TeamId enforces one statistics row per team per match and also serves as the index backing the WHERE MatchId = @MatchId lookup in GetMatchTeamStatistics, so no separate MatchId index is needed.
-- UQ_MatchTeamStatistics_MatchId_IsHomeTeam allows at most one home row and one away row per match; since V1_0_0_16 dropped HomeTeamId/AwayTeamId from Matches, this pairing is the only record of which team played at home.
-- IX_MatchTeamStatistics_TeamId backs the cascade from Teams.
-- Possession is a whole-number percentage constrained to 0-100 in the database as well as in the validators, so the two agree.
-- ShotsOnTarget is a subset of ShotsTotal, so CK_MatchTeamStatistics_ShotsOnTargetWithinTotal keeps it from exceeding the total; the validators enforce the same rule.
-- CK_MatchTeamStatistics_ShotsNonNegative is one constraint over both shot counters; the validators report each field separately, so the database check is only the backstop.
CREATE TABLE dbo.MatchTeamStatistics (
    Id            INT              IDENTITY(1,1)    NOT NULL,
    TeamId        INT                               NOT NULL,
    MatchId       INT                               NOT NULL,
    IsHomeTeam    BIT                               NOT NULL,
    ShotsTotal    INT                               NOT NULL,
    ShotsOnTarget INT                               NOT NULL,
    Possession    INT                               NOT NULL,
    CreatedAt     DATETIME2(7)                      NOT NULL,
    ModifiedAt    DATETIME2(7)                      NOT NULL,
    CreatedBy     UNIQUEIDENTIFIER                  NOT NULL,
    ModifiedBy    UNIQUEIDENTIFIER                  NOT NULL,
    CONSTRAINT PK_MatchTeamStatistics PRIMARY KEY CLUSTERED (Id),
    CONSTRAINT FK_MatchTeamStatistics_Teams FOREIGN KEY (TeamId) REFERENCES dbo.Teams (Id) ON DELETE CASCADE,
    CONSTRAINT FK_MatchTeamStatistics_Matches FOREIGN KEY (MatchId) REFERENCES dbo.Matches (Id) ON DELETE CASCADE,
    CONSTRAINT UQ_MatchTeamStatistics_MatchId_TeamId UNIQUE (MatchId, TeamId),
    CONSTRAINT UQ_MatchTeamStatistics_MatchId_IsHomeTeam UNIQUE (MatchId, IsHomeTeam),
    CONSTRAINT CK_MatchTeamStatistics_Possession CHECK (Possession BETWEEN 0 AND 100),
    CONSTRAINT CK_MatchTeamStatistics_ShotsOnTargetWithinTotal CHECK (ShotsOnTarget <= ShotsTotal),
    CONSTRAINT CK_MatchTeamStatistics_ShotsNonNegative CHECK (ShotsTotal >= 0 AND ShotsOnTarget >= 0)
);

CREATE NONCLUSTERED INDEX IX_MatchTeamStatistics_TeamId ON dbo.MatchTeamStatistics (TeamId);
