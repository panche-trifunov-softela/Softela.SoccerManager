-- FK_MatchTeamTactics_Matches and FK_MatchTeamTactics_Teams cascade: the only foreign key Teams carries is FK_Teams_Stadiums (SET NULL) and Stadiums reaches nothing else, while the only cascading path into Matches is Leagues -> Seasons -> Matches (its Divisions key is NO ACTION), so each parent reaches this table by a single path and SQL Server accepts both cascades.
-- The four foreign keys to Players are NO ACTION: SQL Server rejects more than one cascading foreign key from a table to the same parent as multiple cascade paths, so a player who is a captain or a set-piece taker in a tactic cannot be deleted while that tactic row exists.
-- UQ_MatchTeamTactics_MatchId_TeamId enforces one tactic per team per match and also serves as the index backing the WHERE MatchId = @MatchId lookup in GetMatchTeamTactics, so no separate MatchId index is needed.
-- IX_MatchTeamTactics_TeamId backs the cascade from Teams; the four player indexes back the NO ACTION checks SQL Server runs when a player is deleted.
-- Mentality, Tempo, Passing, Width, Pressing, Tackling and AttackingSide are TINYINT holding the enum of the same name; every enum starts at 1, so a value that was never set is rejected by validation rather than stored as the first member. There is no CHECK constraint, matching the Morale precedent.
-- The same player may hold several roles (for example captain and penalty taker), so there is no constraint that the four player columns differ.
CREATE TABLE dbo.MatchTeamTactics (
    Id                      INT              IDENTITY(1,1)    NOT NULL,
    MatchId                 INT                               NOT NULL,
    TeamId                  INT                               NOT NULL,
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
    CONSTRAINT PK_MatchTeamTactics PRIMARY KEY CLUSTERED (Id),
    CONSTRAINT FK_MatchTeamTactics_Matches FOREIGN KEY (MatchId) REFERENCES dbo.Matches (Id) ON DELETE CASCADE,
    CONSTRAINT FK_MatchTeamTactics_Teams FOREIGN KEY (TeamId) REFERENCES dbo.Teams (Id) ON DELETE CASCADE,
    CONSTRAINT FK_MatchTeamTactics_PenaltyTakerPlayers FOREIGN KEY (PenaltyTakerPlayerId) REFERENCES dbo.Players (Id),
    CONSTRAINT FK_MatchTeamTactics_FreeKickTakerPlayers FOREIGN KEY (FreeKickTakerPlayerId) REFERENCES dbo.Players (Id),
    CONSTRAINT FK_MatchTeamTactics_CornerKickTakerPlayers FOREIGN KEY (CornerKickTakerPlayerId) REFERENCES dbo.Players (Id),
    CONSTRAINT FK_MatchTeamTactics_CaptainPlayers FOREIGN KEY (CaptainPlayerId) REFERENCES dbo.Players (Id),
    CONSTRAINT UQ_MatchTeamTactics_MatchId_TeamId UNIQUE (MatchId, TeamId)
);

CREATE NONCLUSTERED INDEX IX_MatchTeamTactics_TeamId ON dbo.MatchTeamTactics (TeamId);
CREATE NONCLUSTERED INDEX IX_MatchTeamTactics_PenaltyTakerPlayerId ON dbo.MatchTeamTactics (PenaltyTakerPlayerId);
CREATE NONCLUSTERED INDEX IX_MatchTeamTactics_FreeKickTakerPlayerId ON dbo.MatchTeamTactics (FreeKickTakerPlayerId);
CREATE NONCLUSTERED INDEX IX_MatchTeamTactics_CornerKickTakerPlayerId ON dbo.MatchTeamTactics (CornerKickTakerPlayerId);
CREATE NONCLUSTERED INDEX IX_MatchTeamTactics_CaptainPlayerId ON dbo.MatchTeamTactics (CaptainPlayerId);
