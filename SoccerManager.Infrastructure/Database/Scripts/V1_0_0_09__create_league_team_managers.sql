-- ManagerId deliberately carries no foreign key: dbo.Managers does not exist yet, and an FK to a missing table would make the Evolve migration fail at startup. The constraint should be added by the manager migration later.
-- StartDate and EndDate are DATE, not DATETIME2, so they carry no time and no zone, the same as Players.DateOfBirth.
-- EndDate is NULL while the tenure is current and is set only when it actually ends. CK_LeagueTeamManagers_CurrentHasNoEndDate and CK_LeagueTeamManagers_EndDateAfterStart mirror the rules in the validators, so the two agree.
-- Both foreign keys cascade: Leagues and Teams are independent root tables (Teams has no foreign keys of its own), so there is no cycle and no shared ancestor for SQL Server to reject.
-- UQ_LeagueTeamManagers_LeagueId_TeamId_ManagerId_StartDate leads on (LeagueId, TeamId), so it also backs the WHERE LeagueId = @LeagueId AND TeamId = @TeamId lookup in GetLeagueTeamManagers and no separate index is needed.
-- UX_LeagueTeamManagers_LeagueId_TeamId_Current is a filtered unique index, so a team can have at most one current manager per league; a second current row is rejected by the database rather than reconciled by the application.
CREATE TABLE dbo.LeagueTeamManagers (
    Id         INT              IDENTITY(1,1)    NOT NULL,
    LeagueId   INT                               NOT NULL,
    TeamId     INT                               NOT NULL,
    ManagerId  INT                               NOT NULL,
    StartDate  DATE                              NOT NULL,
    EndDate    DATE                                  NULL,
    IsCurrent  BIT                               NOT NULL,
    CreatedAt  DATETIME2(7)                      NOT NULL,
    ModifiedAt DATETIME2(7)                      NOT NULL,
    CreatedBy  UNIQUEIDENTIFIER                  NOT NULL,
    ModifiedBy UNIQUEIDENTIFIER                  NOT NULL,
    CONSTRAINT PK_LeagueTeamManagers PRIMARY KEY CLUSTERED (Id),
    CONSTRAINT FK_LeagueTeamManagers_Leagues FOREIGN KEY (LeagueId) REFERENCES dbo.Leagues (Id) ON DELETE CASCADE,
    CONSTRAINT FK_LeagueTeamManagers_Teams FOREIGN KEY (TeamId) REFERENCES dbo.Teams (Id) ON DELETE CASCADE,
    CONSTRAINT UQ_LeagueTeamManagers_LeagueId_TeamId_ManagerId_StartDate UNIQUE (LeagueId, TeamId, ManagerId, StartDate),
    CONSTRAINT CK_LeagueTeamManagers_EndDateAfterStart CHECK (EndDate IS NULL OR EndDate >= StartDate),
    CONSTRAINT CK_LeagueTeamManagers_CurrentHasNoEndDate CHECK (IsCurrent = 0 OR EndDate IS NULL)
);

CREATE UNIQUE NONCLUSTERED INDEX UX_LeagueTeamManagers_LeagueId_TeamId_Current
    ON dbo.LeagueTeamManagers (LeagueId, TeamId)
    WHERE IsCurrent = 1;
