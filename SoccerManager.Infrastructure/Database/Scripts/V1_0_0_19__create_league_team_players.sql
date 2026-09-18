-- All three foreign keys cascade: Leagues, Teams and Players are independent roots for cascade purposes (Teams' only foreign key is FK_Teams_Stadiums ON DELETE SET NULL, and Stadiums reaches nothing else), so each parent reaches this table by a single path and SQL Server accepts every cascade.
-- UQ_LeagueTeamPlayers_LeagueId_PlayerId registers a player with at most one team per league; it leads on LeagueId, so it also backs the cascade from Leagues.
-- UQ_LeagueTeamPlayers_TeamId_LeagueId_SquadNumber gives each squad number to at most one player per team per league; its (TeamId, LeagueId) prefix also backs the WHERE LeagueId = @LeagueId AND TeamId = @TeamId lookup in GetLeagueTeamPlayers and the cascade from Teams, so no separate index is needed for either.
-- IX_LeagueTeamPlayers_PlayerId backs the cascade from Players.
-- Morale is TINYINT holding the Morale enum value, where 1 is VeryConcerned and 5 is VerySatisfied. Zero is deliberately unused, so a value that was never set is rejected rather than stored as VeryConcerned.
-- ContractSalaryPerWeek and TransfermarketValue are BIGINT because both are whole currency amounts that can exceed INT's range; the application maps them to long.
-- ContractLength (1-6) and SquadNumber (1-99) are constrained in the database as well as in the validators, so the two agree.
-- WantedTotalAppearances includes the starting ones, so CK_LeagueTeamPlayers_StarterWithinTotalAppearances keeps WantedStarterAppearances from exceeding it; the validators enforce the same rule.
-- The two non-negative constraints each cover several columns; the validators report each field separately, so the database checks are only the backstop.
CREATE TABLE dbo.LeagueTeamPlayers (
    Id                       INT              IDENTITY(1,1)    NOT NULL,
    LeagueId                 INT                               NOT NULL,
    TeamId                   INT                               NOT NULL,
    PlayerId                 INT                               NOT NULL,
    ContractLength           INT                               NOT NULL,
    ContractSalaryPerWeek    BIGINT                            NOT NULL,
    SquadNumber              INT                               NOT NULL,
    FansFavoritePlayer       BIT                               NOT NULL,
    Morale                   TINYINT                           NOT NULL,
    TransfermarketValue      BIGINT                            NOT NULL,
    WantedStarterAppearances INT                               NOT NULL,
    WantedTotalAppearances   INT                               NOT NULL,
    CreatedAt                DATETIME2(7)                      NOT NULL,
    ModifiedAt               DATETIME2(7)                      NOT NULL,
    CreatedBy                UNIQUEIDENTIFIER                  NOT NULL,
    ModifiedBy               UNIQUEIDENTIFIER                  NOT NULL,
    CONSTRAINT PK_LeagueTeamPlayers PRIMARY KEY CLUSTERED (Id),
    CONSTRAINT FK_LeagueTeamPlayers_Leagues FOREIGN KEY (LeagueId) REFERENCES dbo.Leagues (Id) ON DELETE CASCADE,
    CONSTRAINT FK_LeagueTeamPlayers_Teams FOREIGN KEY (TeamId) REFERENCES dbo.Teams (Id) ON DELETE CASCADE,
    CONSTRAINT FK_LeagueTeamPlayers_Players FOREIGN KEY (PlayerId) REFERENCES dbo.Players (Id) ON DELETE CASCADE,
    CONSTRAINT UQ_LeagueTeamPlayers_LeagueId_PlayerId UNIQUE (LeagueId, PlayerId),
    CONSTRAINT UQ_LeagueTeamPlayers_TeamId_LeagueId_SquadNumber UNIQUE (TeamId, LeagueId, SquadNumber),
    CONSTRAINT CK_LeagueTeamPlayers_ContractLength CHECK (ContractLength BETWEEN 1 AND 6),
    CONSTRAINT CK_LeagueTeamPlayers_SquadNumber CHECK (SquadNumber BETWEEN 1 AND 99),
    CONSTRAINT CK_LeagueTeamPlayers_AmountsNonNegative CHECK (ContractSalaryPerWeek >= 0 AND TransfermarketValue >= 0),
    CONSTRAINT CK_LeagueTeamPlayers_AppearancesNonNegative CHECK (WantedStarterAppearances >= 0 AND WantedTotalAppearances >= 0),
    CONSTRAINT CK_LeagueTeamPlayers_StarterWithinTotalAppearances CHECK (WantedStarterAppearances <= WantedTotalAppearances)
);

CREATE NONCLUSTERED INDEX IX_LeagueTeamPlayers_PlayerId ON dbo.LeagueTeamPlayers (PlayerId);
