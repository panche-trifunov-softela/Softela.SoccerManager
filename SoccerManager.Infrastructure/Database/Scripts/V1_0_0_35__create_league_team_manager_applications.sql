-- A manager's application to manage a team in a league. It stays Pending until it is answered, and accepting it creates the dbo.LeagueTeamManagers appointment in the same transaction.
-- Status is a TINYINT holding the ApplicationStatus enum (Pending 1, Accepted 2, Rejected 3). Like every other enum column it has no CHECK on its range, and DF_LeagueTeamManagerApplications_Status makes a new row Pending.
-- ResponseDate is NULL while the application is pending and holds the UTC moment it was answered. CK_LeagueTeamManagerApplications_ResponseDateMatchesStatus keeps the two in step.
-- All three foreign keys cascade: Leagues, Teams and Managers are independent cascade roots and nothing references this table, so each parent reaches it by exactly one path, as for dbo.LeagueTeamManagers.
-- UX_LeagueTeamManagerApplications_LeagueId_TeamId_ManagerId_Pending is a filtered unique index, so a manager has at most one pending application per team per league.
-- IX_LeagueTeamManagerApplications_LeagueId_TeamId backs the Leagues cascade and the GetLeagueTeamManagerApplications lookup; the TeamId and ManagerId indexes back the other two cascades.
CREATE TABLE dbo.LeagueTeamManagerApplications (
    Id           INT              IDENTITY(1,1)    NOT NULL,
    LeagueId     INT                               NOT NULL,
    TeamId       INT                               NOT NULL,
    ManagerId    INT                               NOT NULL,
    Status       TINYINT                           NOT NULL CONSTRAINT DF_LeagueTeamManagerApplications_Status DEFAULT 1,
    ResponseDate DATETIME2(7)                          NULL,
    CreatedAt    DATETIME2(7)                      NOT NULL,
    ModifiedAt   DATETIME2(7)                      NOT NULL,
    CreatedBy    UNIQUEIDENTIFIER                  NOT NULL,
    ModifiedBy   UNIQUEIDENTIFIER                  NOT NULL,
    CONSTRAINT PK_LeagueTeamManagerApplications PRIMARY KEY CLUSTERED (Id),
    CONSTRAINT FK_LeagueTeamManagerApplications_Leagues FOREIGN KEY (LeagueId) REFERENCES dbo.Leagues (Id) ON DELETE CASCADE,
    CONSTRAINT FK_LeagueTeamManagerApplications_Teams FOREIGN KEY (TeamId) REFERENCES dbo.Teams (Id) ON DELETE CASCADE,
    CONSTRAINT FK_LeagueTeamManagerApplications_Managers FOREIGN KEY (ManagerId) REFERENCES dbo.Managers (Id) ON DELETE CASCADE,
    CONSTRAINT CK_LeagueTeamManagerApplications_ResponseDateMatchesStatus CHECK ((Status = 1 AND ResponseDate IS NULL) OR (Status <> 1 AND ResponseDate IS NOT NULL))
);

CREATE UNIQUE NONCLUSTERED INDEX UX_LeagueTeamManagerApplications_LeagueId_TeamId_ManagerId_Pending
    ON dbo.LeagueTeamManagerApplications (LeagueId, TeamId, ManagerId)
    WHERE Status = 1;

CREATE NONCLUSTERED INDEX IX_LeagueTeamManagerApplications_LeagueId_TeamId ON dbo.LeagueTeamManagerApplications (LeagueId, TeamId);

CREATE NONCLUSTERED INDEX IX_LeagueTeamManagerApplications_TeamId ON dbo.LeagueTeamManagerApplications (TeamId);

CREATE NONCLUSTERED INDEX IX_LeagueTeamManagerApplications_ManagerId ON dbo.LeagueTeamManagerApplications (ManagerId);
