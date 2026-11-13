-- dbo.LeagueTeamPlayerInjuries holds one row per injury of a squad row (dbo.LeagueTeamPlayers): its name, when it started and ended, and how serious it was.
-- The only foreign key, FK_LeagueTeamPlayerInjuries_LeagueTeamPlayers, cascades: releasing a player removes the player's injuries, and a league, club or Team delete reaches them through the squad row.
-- The table has no club column on purpose, so there is no second cascade path from dbo.LeagueTeams (Msg 1785) and dbo.DeleteLeagueTeam needs no guard for injuries: a club that still has a squad is already refused.
-- InjuryScale holds a SoccerManager.Domain.Enums.InjuryScale value (1 Minor, 2 Moderate, 3 Serious), and CK_LeagueTeamPlayerInjuries_InjuryScaleKnown keeps any other value out, so a new level needs a new script as well as a new enum member.
-- InjuryStartDate and InjuryEndDate are DATETIME2(7) holding UTC, the same convention as dbo.Matches.StartDateTime. The end is never before the start (CK_LeagueTeamPlayerInjuries_EndNotBeforeStart).
-- The foreign key column has its own index, which also backs the cascade.
CREATE TABLE dbo.LeagueTeamPlayerInjuries (
    Id                 INT              IDENTITY(1,1)    NOT NULL,
    LeagueTeamPlayerId INT                               NOT NULL,
    Name               NVARCHAR(100)                     NOT NULL,
    InjuryStartDate    DATETIME2(7)                      NOT NULL,
    InjuryEndDate      DATETIME2(7)                      NOT NULL,
    InjuryScale        TINYINT                           NOT NULL,
    CreatedAt          DATETIME2(7)                      NOT NULL,
    ModifiedAt         DATETIME2(7)                      NOT NULL,
    CreatedBy          UNIQUEIDENTIFIER                  NOT NULL,
    ModifiedBy         UNIQUEIDENTIFIER                  NOT NULL,
    CONSTRAINT PK_LeagueTeamPlayerInjuries PRIMARY KEY CLUSTERED (Id),
    CONSTRAINT FK_LeagueTeamPlayerInjuries_LeagueTeamPlayers FOREIGN KEY (LeagueTeamPlayerId) REFERENCES dbo.LeagueTeamPlayers (Id) ON DELETE CASCADE,
    CONSTRAINT CK_LeagueTeamPlayerInjuries_EndNotBeforeStart CHECK (InjuryEndDate >= InjuryStartDate),
    CONSTRAINT CK_LeagueTeamPlayerInjuries_InjuryScaleKnown CHECK (InjuryScale BETWEEN 1 AND 3)
);

CREATE NONCLUSTERED INDEX IX_LeagueTeamPlayerInjuries_LeagueTeamPlayerId ON dbo.LeagueTeamPlayerInjuries (LeagueTeamPlayerId);
