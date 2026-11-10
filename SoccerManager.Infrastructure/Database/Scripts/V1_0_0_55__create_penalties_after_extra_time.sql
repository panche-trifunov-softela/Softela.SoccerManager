-- dbo.PenaltiesAfterExtraTime holds one row per kick of a penalty shootout after extra time, in kick order (KickNumber), because a statistic row has no minute; the club with more scored kicks wins the shootout.
-- FK_PenaltiesAfterExtraTime_Matches cascades: the only cascading path into Matches is Leagues -> Seasons -> Matches, so a match delete removes its kicks through a single path.
-- FK_PenaltiesAfterExtraTime_LeagueTeams does not cascade, as in V1_0_0_41: Leagues already cascade into this table through Seasons and Matches, and a second path through LeagueTeams would be rejected. A roster entry with kicks can therefore not be deleted, and dbo.DeleteLeagueTeam checks for them.
-- FK_PenaltiesAfterExtraTime_Players cascades, as FK_MatchPlayerStatistics_Players does: Players is reached only through SET NULL, which ends a cascade there.
-- UQ_PenaltiesAfterExtraTime_MatchId_KickNumber gives each place in the order one kick and also backs the by-match lookup.
-- CK_PenaltiesAfterExtraTime_KickNumber numbers the kicks from 1.
-- IX_PenaltiesAfterExtraTime_LeagueTeamId backs the foreign key and the roster-removal check; IX_PenaltiesAfterExtraTime_PlayerId backs the cascade from Players.
-- The shootout rules (alternating clubs, five kicks each, sudden death) are the writer's to keep; nothing in the database checks them.
CREATE TABLE dbo.PenaltiesAfterExtraTime (
    Id           INT              IDENTITY(1,1)    NOT NULL,
    MatchId      INT                               NOT NULL,
    KickNumber   INT                               NOT NULL,
    LeagueTeamId INT                               NOT NULL,
    PlayerId     INT                               NOT NULL,
    IsScored     BIT                               NOT NULL,
    CreatedAt    DATETIME2(7)                      NOT NULL,
    ModifiedAt   DATETIME2(7)                      NOT NULL,
    CreatedBy    UNIQUEIDENTIFIER                  NOT NULL,
    ModifiedBy   UNIQUEIDENTIFIER                  NOT NULL,
    CONSTRAINT PK_PenaltiesAfterExtraTime PRIMARY KEY CLUSTERED (Id),
    CONSTRAINT FK_PenaltiesAfterExtraTime_Matches FOREIGN KEY (MatchId) REFERENCES dbo.Matches (Id) ON DELETE CASCADE,
    CONSTRAINT FK_PenaltiesAfterExtraTime_LeagueTeams FOREIGN KEY (LeagueTeamId) REFERENCES dbo.LeagueTeams (Id),
    CONSTRAINT FK_PenaltiesAfterExtraTime_Players FOREIGN KEY (PlayerId) REFERENCES dbo.Players (Id) ON DELETE CASCADE,
    CONSTRAINT UQ_PenaltiesAfterExtraTime_MatchId_KickNumber UNIQUE (MatchId, KickNumber),
    CONSTRAINT CK_PenaltiesAfterExtraTime_KickNumber CHECK (KickNumber >= 1)
);

CREATE NONCLUSTERED INDEX IX_PenaltiesAfterExtraTime_LeagueTeamId ON dbo.PenaltiesAfterExtraTime (LeagueTeamId);
CREATE NONCLUSTERED INDEX IX_PenaltiesAfterExtraTime_PlayerId ON dbo.PenaltiesAfterExtraTime (PlayerId);
