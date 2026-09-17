-- UserId is the Keycloak user id (the sub claim), the same value CreatedBy/ModifiedBy hold, and deliberately carries no foreign key: Keycloak's store is a separate database, so SQL Server cannot reference it, and integrity here is by convention only.
-- UQ_Managers_UserId allows one manager profile per Keycloak user.
-- ImageUrl NVARCHAR(500) matches the application's CreateManagerValidator/UpdateManagerValidator MaximumLength(500); it holds a URL only, no image bytes.
-- FK_LeagueTeamManagers_Managers is the constraint V1_0_0_09 deferred until this table existed. It cascades like that table's two existing foreign keys, and Managers is an independent root table so there is no multiple-cascade-path conflict.
CREATE TABLE dbo.Managers (
    Id         INT              IDENTITY(1,1)    NOT NULL,
    UserId     UNIQUEIDENTIFIER                  NOT NULL,
    ImageUrl   NVARCHAR(500)                         NULL,
    CreatedAt  DATETIME2(7)                      NOT NULL,
    ModifiedAt DATETIME2(7)                      NOT NULL,
    CreatedBy  UNIQUEIDENTIFIER                  NOT NULL,
    ModifiedBy UNIQUEIDENTIFIER                  NOT NULL,
    CONSTRAINT PK_Managers PRIMARY KEY CLUSTERED (Id),
    CONSTRAINT UQ_Managers_UserId UNIQUE (UserId)
);

ALTER TABLE dbo.LeagueTeamManagers
    ADD CONSTRAINT FK_LeagueTeamManagers_Managers FOREIGN KEY (ManagerId) REFERENCES dbo.Managers (Id) ON DELETE CASCADE;
