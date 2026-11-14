-- dbo.MatchPlayerAbsences holds one row per player who missed a match: the club the player was missing from (LeagueTeamId) and why (Reason), injured or suspended.
-- FK_MatchPlayerAbsences_Matches cascades: the only cascading path into Matches is Leagues -> Seasons -> Matches, so a match delete removes its absences through a single path.
-- FK_MatchPlayerAbsences_LeagueTeams does not cascade, as in V1_0_0_41: Leagues already cascade into this table through Seasons and Matches, and a second path through LeagueTeams would be rejected. A roster entry with absences can therefore not be deleted, and dbo.DeleteLeagueTeam checks for them.
-- FK_MatchPlayerAbsences_Players cascades, as FK_MatchPlayerStatistics_Players does: Players is reached only through SET NULL, which ends a cascade there.
-- UQ_MatchPlayerAbsences_MatchId_PlayerId gives each player one absence per match and also backs the by-match lookup.
-- CK_MatchPlayerAbsences_ReasonKnown accepts only the SoccerManager.Domain.Enums.MatchAbsenceReason values (1 Injured, 2 Suspended), so a new reason needs a new script as well as a new enum member.
-- IX_MatchPlayerAbsences_LeagueTeamId backs the foreign key and the roster-removal check; IX_MatchPlayerAbsences_PlayerId backs the cascade from Players.
-- The other absence rules (the player is in the club's squad and played no minutes in the match) are the writer's to keep; dbo.InsertMatchPlayerAbsence and dbo.UpdateMatchPlayerAbsence also refuse a club that does not play the match.
CREATE TABLE dbo.MatchPlayerAbsences (
    Id           INT              IDENTITY(1,1)    NOT NULL,
    MatchId      INT                               NOT NULL,
    LeagueTeamId INT                               NOT NULL,
    PlayerId     INT                               NOT NULL,
    Reason       TINYINT                           NOT NULL,
    CreatedAt    DATETIME2(7)                      NOT NULL,
    ModifiedAt   DATETIME2(7)                      NOT NULL,
    CreatedBy    UNIQUEIDENTIFIER                  NOT NULL,
    ModifiedBy   UNIQUEIDENTIFIER                  NOT NULL,
    CONSTRAINT PK_MatchPlayerAbsences PRIMARY KEY CLUSTERED (Id),
    CONSTRAINT FK_MatchPlayerAbsences_Matches FOREIGN KEY (MatchId) REFERENCES dbo.Matches (Id) ON DELETE CASCADE,
    CONSTRAINT FK_MatchPlayerAbsences_LeagueTeams FOREIGN KEY (LeagueTeamId) REFERENCES dbo.LeagueTeams (Id),
    CONSTRAINT FK_MatchPlayerAbsences_Players FOREIGN KEY (PlayerId) REFERENCES dbo.Players (Id) ON DELETE CASCADE,
    CONSTRAINT UQ_MatchPlayerAbsences_MatchId_PlayerId UNIQUE (MatchId, PlayerId),
    CONSTRAINT CK_MatchPlayerAbsences_ReasonKnown CHECK (Reason BETWEEN 1 AND 2)
);

CREATE NONCLUSTERED INDEX IX_MatchPlayerAbsences_LeagueTeamId ON dbo.MatchPlayerAbsences (LeagueTeamId);
CREATE NONCLUSTERED INDEX IX_MatchPlayerAbsences_PlayerId ON dbo.MatchPlayerAbsences (PlayerId);
