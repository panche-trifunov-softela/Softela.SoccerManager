-- dbo.NationalTeamMatches holds one row per World Cup match between two national teams in a league's season, qualifiers and finals alike; it has no CompetitionId, because the table holds only the World Cup.
-- FK_NationalTeamMatches_Seasons cascades: the only cascading path into this table is Leagues -> Seasons -> NationalTeamMatches, so a season or league delete removes its national matches, as it removes its club matches.
-- FK_NationalTeamMatches_HomeNationalTeams and FK_NationalTeamMatches_AwayNationalTeams do not cascade: two cascading paths from dbo.NationalTeams into one table would be rejected, as V1_0_0_13 says of the team keys, and a national team must not take a World Cup with it. A national team with matches can therefore not be deleted.
-- CK_NationalTeamMatches_DifferentNations keeps a national team from playing itself, the rule V1_0_0_13 gave dbo.Matches (CK_Matches_DifferentTeams) before V1_0_0_16 moved the clubs to dbo.MatchTeamStatistics.
-- Stage holds a SoccerManager.Domain.Enums.NationalTeamMatchStage value (1 QualifierGroup, 2 FinalsGroup, 3 RoundOf16, 4 QuarterFinal, 5 SemiFinal, 6 ThirdPlace, 7 Final), and CK_NationalTeamMatches_StageKnown keeps any other value out, so a new stage needs a new script as well as a new enum member.
-- GroupNumber, RoundNumber and Leg place a match within its stage: its group, its matchday and its leg. Each is NULL where it does not apply, and which stage uses which is the writer's to keep; the CHECKs only number them from 1 and keep Leg to 1 or 2, as V1_0_0_57 does for dbo.Matches.
-- CK_NationalTeamMatches_FinishedHasStarted is the rule V1_0_0_56 gave dbo.Matches: a finished match has started. DF_NationalTeamMatches_IsStarted and DF_NationalTeamMatches_IsFinished make a new match unplayed.
-- StartDateTime holds UTC, as dbo.Matches.StartDateTime does (V1_0_0_13).
-- IX_NationalTeamMatches_SeasonId backs the cascade from Seasons and the by-season read; IX_NationalTeamMatches_HomeNationalTeamId and IX_NationalTeamMatches_AwayNationalTeamId back the check a national team delete makes.
CREATE TABLE dbo.NationalTeamMatches (
    Id                 INT              IDENTITY(1,1)    NOT NULL,
    SeasonId           INT                               NOT NULL,
    HomeNationalTeamId INT                               NOT NULL,
    AwayNationalTeamId INT                               NOT NULL,
    StartDateTime      DATETIME2(7)                      NOT NULL,
    Stage              TINYINT                           NOT NULL,
    GroupNumber        TINYINT                           NULL,
    RoundNumber        TINYINT                           NULL,
    Leg                TINYINT                           NULL,
    IsStarted          BIT                               NOT NULL CONSTRAINT DF_NationalTeamMatches_IsStarted DEFAULT 0,
    IsFinished         BIT                               NOT NULL CONSTRAINT DF_NationalTeamMatches_IsFinished DEFAULT 0,
    CreatedAt          DATETIME2(7)                      NOT NULL,
    ModifiedAt         DATETIME2(7)                      NOT NULL,
    CreatedBy          UNIQUEIDENTIFIER                  NOT NULL,
    ModifiedBy         UNIQUEIDENTIFIER                  NOT NULL,
    CONSTRAINT PK_NationalTeamMatches PRIMARY KEY CLUSTERED (Id),
    CONSTRAINT FK_NationalTeamMatches_Seasons FOREIGN KEY (SeasonId) REFERENCES dbo.Seasons (Id) ON DELETE CASCADE,
    CONSTRAINT FK_NationalTeamMatches_HomeNationalTeams FOREIGN KEY (HomeNationalTeamId) REFERENCES dbo.NationalTeams (Id),
    CONSTRAINT FK_NationalTeamMatches_AwayNationalTeams FOREIGN KEY (AwayNationalTeamId) REFERENCES dbo.NationalTeams (Id),
    CONSTRAINT CK_NationalTeamMatches_DifferentNations CHECK (HomeNationalTeamId <> AwayNationalTeamId),
    CONSTRAINT CK_NationalTeamMatches_StageKnown CHECK (Stage BETWEEN 1 AND 7),
    CONSTRAINT CK_NationalTeamMatches_GroupNumberPositive CHECK (GroupNumber >= 1),
    CONSTRAINT CK_NationalTeamMatches_RoundNumberPositive CHECK (RoundNumber >= 1),
    CONSTRAINT CK_NationalTeamMatches_LegFirstOrSecond CHECK (Leg IN (1, 2)),
    CONSTRAINT CK_NationalTeamMatches_FinishedHasStarted CHECK (IsFinished = 0 OR IsStarted = 1)
);

CREATE NONCLUSTERED INDEX IX_NationalTeamMatches_SeasonId ON dbo.NationalTeamMatches (SeasonId);
CREATE NONCLUSTERED INDEX IX_NationalTeamMatches_HomeNationalTeamId ON dbo.NationalTeamMatches (HomeNationalTeamId);
CREATE NONCLUSTERED INDEX IX_NationalTeamMatches_AwayNationalTeamId ON dbo.NationalTeamMatches (AwayNationalTeamId);
