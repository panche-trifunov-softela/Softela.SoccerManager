-- The six statistic columns default to 0 so a newly created row is a blank record; the insert procedure still passes every value explicitly, so the defaults only matter for rows created outside it.
-- CK_Standings_NonNegative mirrors the GreaterThanOrEqualTo(0) rules in the validators, so the two agree. Note that because Points cannot go negative, a points deduction can never take a team below zero.
-- Only FK_Standings_Seasons cascades. Seasons and Divisions BOTH cascade from Leagues, so cascading on DivisionId as well would give Leagues two cascade paths to Standings, which SQL Server rejects outright. Divisions and Teams therefore use the default NO ACTION.
-- UQ_Standings_SeasonId_DivisionId_TeamId leads on (SeasonId, DivisionId), so it also backs the GetStandings lookup and no separate index is needed.
-- Nothing enforces that the season and the division belong to the same league; Seasons.LeagueId and Divisions.LeagueId are independent, and a plain foreign key cannot express that pairing.
CREATE TABLE dbo.Standings (
    Id           INT              IDENTITY(1,1)    NOT NULL,
    SeasonId     INT                               NOT NULL,
    DivisionId   INT                               NOT NULL,
    TeamId       INT                               NOT NULL,
    Points       INT                               NOT NULL DEFAULT 0,
    GoalsFor     INT                               NOT NULL DEFAULT 0,
    GoalsAgainst INT                               NOT NULL DEFAULT 0,
    Wins         INT                               NOT NULL DEFAULT 0,
    Draws        INT                               NOT NULL DEFAULT 0,
    Losses       INT                               NOT NULL DEFAULT 0,
    CreatedAt    DATETIME2(7)                      NOT NULL,
    ModifiedAt   DATETIME2(7)                      NOT NULL,
    CreatedBy    UNIQUEIDENTIFIER                  NOT NULL,
    ModifiedBy   UNIQUEIDENTIFIER                  NOT NULL,
    CONSTRAINT PK_Standings PRIMARY KEY CLUSTERED (Id),
    CONSTRAINT FK_Standings_Seasons FOREIGN KEY (SeasonId) REFERENCES dbo.Seasons (Id) ON DELETE CASCADE,
    CONSTRAINT FK_Standings_Divisions FOREIGN KEY (DivisionId) REFERENCES dbo.Divisions (Id),
    CONSTRAINT FK_Standings_Teams FOREIGN KEY (TeamId) REFERENCES dbo.Teams (Id),
    CONSTRAINT UQ_Standings_SeasonId_DivisionId_TeamId UNIQUE (SeasonId, DivisionId, TeamId),
    CONSTRAINT CK_Standings_NonNegative CHECK (Points >= 0 AND GoalsFor >= 0 AND GoalsAgainst >= 0 AND Wins >= 0 AND Draws >= 0 AND Losses >= 0)
);
