-- dbo.MatchTeamTacticSubstitutions holds a club's substitutions for a match: each row is the tactic the club switches to at one minute (MinuteAt) when the score is in one situation (Situation), with the same seven instructions and four roles as dbo.MatchTeamTactics. A substitution's lineup is in dbo.MatchFormationPlayerPositionSubstitutions under the same match, club, minute and situation.
-- FK_MatchTeamTacticSubstitutions_Matches cascades: the only cascading path into Matches is Leagues -> Seasons -> Matches, so a match delete removes its substitutions through a single path.
-- FK_MatchTeamTacticSubstitutions_LeagueTeams does not cascade, as in V1_0_0_42: Leagues already cascade into this table through Seasons and Matches, and a second path through LeagueTeams would be rejected as multiple cascade paths. A roster entry with substitutions can therefore not be deleted, and dbo.DeleteLeagueTeam checks for them.
-- The four foreign keys to Players are NO ACTION for the reason V1_0_0_20 gives: more than one cascading foreign key from a table to the same parent is rejected as multiple cascade paths, so a player who holds a role in a substitution cannot be deleted while that row exists.
-- UQ_MatchTeamTacticSubstitutions_MatchId_LeagueTeamId_MinuteAt_Situation keeps one substitution per club, match, minute and situation. It leads on MatchId and LeagueTeamId, so it also backs the per-club reads and the range lock a save takes on a club's substitutions for a match.
-- CK_MatchTeamTacticSubstitutions_MinuteAt allows only the minutes a substitution can take effect at: 45, 60 and 75. Situation is a TINYINT holding the enum of the same name, starting at 1, with no CHECK constraint, like every other enum column.
-- IX_MatchTeamTacticSubstitutions_LeagueTeamId backs the foreign key and the roster-removal check; the four player indexes back the NO ACTION checks SQL Server runs when a player is deleted.
CREATE TABLE dbo.MatchTeamTacticSubstitutions (
    Id                      INT              IDENTITY(1,1)    NOT NULL,
    MatchId                 INT                               NOT NULL,
    LeagueTeamId            INT                               NOT NULL,
    MinuteAt                INT                               NOT NULL,
    Situation               TINYINT                           NOT NULL,
    Mentality               TINYINT                           NOT NULL,
    Tempo                   TINYINT                           NOT NULL,
    Passing                 TINYINT                           NOT NULL,
    Width                   TINYINT                           NOT NULL,
    Pressing                TINYINT                           NOT NULL,
    Tackling                TINYINT                           NOT NULL,
    AttackingSide           TINYINT                           NOT NULL,
    PenaltyTakerPlayerId    INT                               NOT NULL,
    FreeKickTakerPlayerId   INT                               NOT NULL,
    CornerKickTakerPlayerId INT                               NOT NULL,
    CaptainPlayerId         INT                               NOT NULL,
    CreatedAt               DATETIME2(7)                      NOT NULL,
    ModifiedAt              DATETIME2(7)                      NOT NULL,
    CreatedBy               UNIQUEIDENTIFIER                  NOT NULL,
    ModifiedBy              UNIQUEIDENTIFIER                  NOT NULL,
    CONSTRAINT PK_MatchTeamTacticSubstitutions PRIMARY KEY CLUSTERED (Id),
    CONSTRAINT FK_MatchTeamTacticSubstitutions_Matches FOREIGN KEY (MatchId) REFERENCES dbo.Matches (Id) ON DELETE CASCADE,
    CONSTRAINT FK_MatchTeamTacticSubstitutions_LeagueTeams FOREIGN KEY (LeagueTeamId) REFERENCES dbo.LeagueTeams (Id),
    CONSTRAINT FK_MatchTeamTacticSubstitutions_PenaltyTakerPlayers FOREIGN KEY (PenaltyTakerPlayerId) REFERENCES dbo.Players (Id),
    CONSTRAINT FK_MatchTeamTacticSubstitutions_FreeKickTakerPlayers FOREIGN KEY (FreeKickTakerPlayerId) REFERENCES dbo.Players (Id),
    CONSTRAINT FK_MatchTeamTacticSubstitutions_CornerKickTakerPlayers FOREIGN KEY (CornerKickTakerPlayerId) REFERENCES dbo.Players (Id),
    CONSTRAINT FK_MatchTeamTacticSubstitutions_CaptainPlayers FOREIGN KEY (CaptainPlayerId) REFERENCES dbo.Players (Id),
    CONSTRAINT UQ_MatchTeamTacticSubstitutions_MatchId_LeagueTeamId_MinuteAt_Situation UNIQUE (MatchId, LeagueTeamId, MinuteAt, Situation),
    CONSTRAINT CK_MatchTeamTacticSubstitutions_MinuteAt CHECK (MinuteAt IN (45, 60, 75))
);

CREATE NONCLUSTERED INDEX IX_MatchTeamTacticSubstitutions_LeagueTeamId ON dbo.MatchTeamTacticSubstitutions (LeagueTeamId);
CREATE NONCLUSTERED INDEX IX_MatchTeamTacticSubstitutions_PenaltyTakerPlayerId ON dbo.MatchTeamTacticSubstitutions (PenaltyTakerPlayerId);
CREATE NONCLUSTERED INDEX IX_MatchTeamTacticSubstitutions_FreeKickTakerPlayerId ON dbo.MatchTeamTacticSubstitutions (FreeKickTakerPlayerId);
CREATE NONCLUSTERED INDEX IX_MatchTeamTacticSubstitutions_CornerKickTakerPlayerId ON dbo.MatchTeamTacticSubstitutions (CornerKickTakerPlayerId);
CREATE NONCLUSTERED INDEX IX_MatchTeamTacticSubstitutions_CaptainPlayerId ON dbo.MatchTeamTacticSubstitutions (CaptainPlayerId);
