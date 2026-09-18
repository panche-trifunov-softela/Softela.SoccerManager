-- Both foreign keys cascade: Players is an independent root with no foreign keys of its own, and the only cascading path into Matches is Leagues -> Seasons -> Matches (its Divisions and Teams keys are NO ACTION), so cascading one hop further gives SQL Server a single path, not multiple.
-- UQ_MatchPlayerStatistics_MatchId_PlayerId enforces one statistics row per player per match and also serves as the index backing the WHERE MatchId = @MatchId lookup in GetMatchPlayerStatistics, so no separate MatchId index is needed.
-- IX_MatchPlayerStatistics_PlayerId backs the cascade from Players.
-- Rating is constrained to 5.0-10.0 in the database as well as in the validators, so the two agree; DECIMAL(3,1) matches the validators' PrecisionScale(3, 1).
-- Goals is the total including penalties scored, so CK_MatchPlayerStatistics_GoalsCoverPenalties keeps it from dropping below PenaltiesScored; the validators enforce the same rule.
-- CK_MatchPlayerStatistics_CountersNonNegative is one constraint over all seven counters; the validators report each field separately, so the database check is only the backstop.
CREATE TABLE dbo.MatchPlayerStatistics (
    Id              INT              IDENTITY(1,1)    NOT NULL,
    PlayerId        INT                               NOT NULL,
    MatchId         INT                               NOT NULL,
    Rating          DECIMAL(3,1)                      NOT NULL,
    IsStarter       BIT                               NOT NULL,
    MinutesPlayed   INT                               NOT NULL,
    Goals           INT                               NOT NULL,
    Assists         INT                               NOT NULL,
    PenaltiesScored INT                               NOT NULL,
    PenaltiesMissed INT                               NOT NULL,
    YellowCards     INT                               NOT NULL,
    RedCards        INT                               NOT NULL,
    CreatedAt       DATETIME2(7)                      NOT NULL,
    ModifiedAt      DATETIME2(7)                      NOT NULL,
    CreatedBy       UNIQUEIDENTIFIER                  NOT NULL,
    ModifiedBy      UNIQUEIDENTIFIER                  NOT NULL,
    CONSTRAINT PK_MatchPlayerStatistics PRIMARY KEY CLUSTERED (Id),
    CONSTRAINT FK_MatchPlayerStatistics_Players FOREIGN KEY (PlayerId) REFERENCES dbo.Players (Id) ON DELETE CASCADE,
    CONSTRAINT FK_MatchPlayerStatistics_Matches FOREIGN KEY (MatchId) REFERENCES dbo.Matches (Id) ON DELETE CASCADE,
    CONSTRAINT UQ_MatchPlayerStatistics_MatchId_PlayerId UNIQUE (MatchId, PlayerId),
    CONSTRAINT CK_MatchPlayerStatistics_Rating CHECK (Rating BETWEEN 5.0 AND 10.0),
    CONSTRAINT CK_MatchPlayerStatistics_GoalsCoverPenalties CHECK (Goals >= PenaltiesScored),
    CONSTRAINT CK_MatchPlayerStatistics_CountersNonNegative CHECK (MinutesPlayed >= 0 AND Goals >= 0 AND Assists >= 0 AND PenaltiesScored >= 0 AND PenaltiesMissed >= 0 AND YellowCards >= 0 AND RedCards >= 0)
);

CREATE NONCLUSTERED INDEX IX_MatchPlayerStatistics_PlayerId ON dbo.MatchPlayerStatistics (PlayerId);
