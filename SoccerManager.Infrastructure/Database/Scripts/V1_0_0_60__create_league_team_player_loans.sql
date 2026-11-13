-- dbo.LeagueTeamPlayerLoans holds one row per loan of a squad row (LeagueTeamPlayerId) to the loan club (ToLeagueTeamId) for one or two seasons (SeasonsLength), starting in a season (SeasonId).
-- LoanFee is what the loan club pays. BuyOptionValue is the price at which the loan club may buy the player when the loan ends; NULL means there is no option to buy.
-- SalaryCoverageInPercent is the share of the player's wage the loan club pays, from 0 to 100.
-- Status is a SoccerManager.Domain.Enums.TransferStatus value (1 Pending, 2 Accepted, 3 Rejected), and CK_LeagueTeamPlayerLoans_StatusKnown keeps any other value out, so a new status needs a new script as well as a new enum member.
-- Money is DECIMAL(18,2), like dbo.Players.Value and dbo.Players.Wage.
-- FK_LeagueTeamPlayerLoans_LeagueTeamPlayers cascades: releasing a player removes his loans. It is the only cascading foreign key here, on purpose.
-- FK_LeagueTeamPlayerLoans_ToLeagueTeams and FK_LeagueTeamPlayerLoans_Seasons do not cascade, as in V1_0_0_59: Leagues and Teams already reach this table through LeagueTeams and LeagueTeamPlayers, and a cascade on a club or season foreign key would be a second path (Msg 1785).
-- dbo.DeleteLeagueTeam therefore refuses while loans go to the club, and dbo.DeleteSeason while loans use the season. A league delete still removes everything in one statement, because the API keeps the loan club and the season in the player's league.
-- One index per foreign key column backs the foreign key, the by-player read and those refusals.
CREATE TABLE dbo.LeagueTeamPlayerLoans (
    Id                      INT              IDENTITY(1,1)    NOT NULL,
    LeagueTeamPlayerId      INT                               NOT NULL,
    ToLeagueTeamId          INT                               NOT NULL,
    SeasonId                INT                               NOT NULL,
    LoanFee                 DECIMAL(18,2)                     NOT NULL,
    SeasonsLength           INT                               NOT NULL,
    BuyOptionValue          DECIMAL(18,2)                     NULL,
    SalaryCoverageInPercent INT                               NOT NULL,
    Status                  TINYINT                           NOT NULL,
    CreatedAt               DATETIME2(7)                      NOT NULL,
    ModifiedAt              DATETIME2(7)                      NOT NULL,
    CreatedBy               UNIQUEIDENTIFIER                  NOT NULL,
    ModifiedBy              UNIQUEIDENTIFIER                  NOT NULL,
    CONSTRAINT PK_LeagueTeamPlayerLoans PRIMARY KEY CLUSTERED (Id),
    CONSTRAINT FK_LeagueTeamPlayerLoans_LeagueTeamPlayers FOREIGN KEY (LeagueTeamPlayerId) REFERENCES dbo.LeagueTeamPlayers (Id) ON DELETE CASCADE,
    CONSTRAINT FK_LeagueTeamPlayerLoans_ToLeagueTeams FOREIGN KEY (ToLeagueTeamId) REFERENCES dbo.LeagueTeams (Id),
    CONSTRAINT FK_LeagueTeamPlayerLoans_Seasons FOREIGN KEY (SeasonId) REFERENCES dbo.Seasons (Id),
    CONSTRAINT CK_LeagueTeamPlayerLoans_LoanFeeNotNegative CHECK (LoanFee >= 0),
    CONSTRAINT CK_LeagueTeamPlayerLoans_SeasonsLength CHECK (SeasonsLength IN (1, 2)),
    CONSTRAINT CK_LeagueTeamPlayerLoans_BuyOptionValueNotNegative CHECK (BuyOptionValue >= 0),
    CONSTRAINT CK_LeagueTeamPlayerLoans_SalaryCoverageInPercent CHECK (SalaryCoverageInPercent BETWEEN 0 AND 100),
    CONSTRAINT CK_LeagueTeamPlayerLoans_StatusKnown CHECK (Status BETWEEN 1 AND 3)
);

CREATE NONCLUSTERED INDEX IX_LeagueTeamPlayerLoans_LeagueTeamPlayerId ON dbo.LeagueTeamPlayerLoans (LeagueTeamPlayerId);
CREATE NONCLUSTERED INDEX IX_LeagueTeamPlayerLoans_ToLeagueTeamId ON dbo.LeagueTeamPlayerLoans (ToLeagueTeamId);
CREATE NONCLUSTERED INDEX IX_LeagueTeamPlayerLoans_SeasonId ON dbo.LeagueTeamPlayerLoans (SeasonId);
