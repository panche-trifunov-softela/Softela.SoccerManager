-- dbo.LeagueTeamPlayerTransfers holds one row per transfer of a squad row (dbo.LeagueTeamPlayers) to the buying club (ToLeagueTeamId) in a season (SeasonId).
-- FromLeagueTeamId is the selling club. The API saves it from the squad row when the transfer is saved, so it survives the player's later move to another club.
-- Only FK_LeagueTeamPlayerTransfers_LeagueTeamPlayers cascades: releasing a player removes his transfers and, through them, their swap rows (V1_0_0_61).
-- Leagues and Teams already reach this table through LeagueTeams -> LeagueTeamPlayers, so a cascade on a club or season foreign key would be a second path (Msg 1785).
-- dbo.DeleteLeagueTeam therefore refuses to remove a club while transfers come from or go to it, and dbo.DeleteSeason refuses to delete a season while transfers use it. A league delete still removes everything in one statement, because the API keeps both clubs and the season in the player's league.
-- Status holds a SoccerManager.Domain.Enums.TransferStatus value (1 Pending, 2 Accepted, 3 Rejected), and CK_LeagueTeamPlayerTransfers_StatusKnown keeps any other value out, so a new status needs a new script as well as a new enum member.
-- BuybackValue and BuybackValidSeasons are set together or not at all (CK_LeagueTeamPlayerTransfers_BuybackBothOrNeither). A CHECK passes when its condition is NULL, so the two range CHECKs judge only a value that is there.
-- Money is DECIMAL(18,2), like dbo.Players.Value and dbo.Players.Wage.
-- Each foreign key column has its own index, which also backs the cascade and the club and season checks above.
CREATE TABLE dbo.LeagueTeamPlayerTransfers (
    Id                  INT              IDENTITY(1,1)    NOT NULL,
    LeagueTeamPlayerId  INT                               NOT NULL,
    FromLeagueTeamId    INT                               NOT NULL,
    ToLeagueTeamId      INT                               NOT NULL,
    SeasonId            INT                               NOT NULL,
    TransferFee         DECIMAL(18,2)                     NOT NULL,
    BuybackValue        DECIMAL(18,2)                     NULL,
    BuybackValidSeasons INT                               NULL,
    Status              TINYINT                           NOT NULL,
    CreatedAt           DATETIME2(7)                      NOT NULL,
    ModifiedAt          DATETIME2(7)                      NOT NULL,
    CreatedBy           UNIQUEIDENTIFIER                  NOT NULL,
    ModifiedBy          UNIQUEIDENTIFIER                  NOT NULL,
    CONSTRAINT PK_LeagueTeamPlayerTransfers PRIMARY KEY CLUSTERED (Id),
    CONSTRAINT FK_LeagueTeamPlayerTransfers_LeagueTeamPlayers FOREIGN KEY (LeagueTeamPlayerId) REFERENCES dbo.LeagueTeamPlayers (Id) ON DELETE CASCADE,
    CONSTRAINT FK_LeagueTeamPlayerTransfers_FromLeagueTeams FOREIGN KEY (FromLeagueTeamId) REFERENCES dbo.LeagueTeams (Id),
    CONSTRAINT FK_LeagueTeamPlayerTransfers_ToLeagueTeams FOREIGN KEY (ToLeagueTeamId) REFERENCES dbo.LeagueTeams (Id),
    CONSTRAINT FK_LeagueTeamPlayerTransfers_Seasons FOREIGN KEY (SeasonId) REFERENCES dbo.Seasons (Id),
    CONSTRAINT CK_LeagueTeamPlayerTransfers_TransferFeeNotNegative CHECK (TransferFee >= 0),
    CONSTRAINT CK_LeagueTeamPlayerTransfers_BuybackValueNotNegative CHECK (BuybackValue >= 0),
    CONSTRAINT CK_LeagueTeamPlayerTransfers_BuybackValidSeasonsPositive CHECK (BuybackValidSeasons >= 1),
    CONSTRAINT CK_LeagueTeamPlayerTransfers_BuybackBothOrNeither CHECK ((BuybackValue IS NULL AND BuybackValidSeasons IS NULL) OR (BuybackValue IS NOT NULL AND BuybackValidSeasons IS NOT NULL)),
    CONSTRAINT CK_LeagueTeamPlayerTransfers_FromAndToDiffer CHECK (FromLeagueTeamId <> ToLeagueTeamId),
    CONSTRAINT CK_LeagueTeamPlayerTransfers_StatusKnown CHECK (Status BETWEEN 1 AND 3)
);

CREATE NONCLUSTERED INDEX IX_LeagueTeamPlayerTransfers_LeagueTeamPlayerId ON dbo.LeagueTeamPlayerTransfers (LeagueTeamPlayerId);
CREATE NONCLUSTERED INDEX IX_LeagueTeamPlayerTransfers_FromLeagueTeamId ON dbo.LeagueTeamPlayerTransfers (FromLeagueTeamId);
CREATE NONCLUSTERED INDEX IX_LeagueTeamPlayerTransfers_ToLeagueTeamId ON dbo.LeagueTeamPlayerTransfers (ToLeagueTeamId);
CREATE NONCLUSTERED INDEX IX_LeagueTeamPlayerTransfers_SeasonId ON dbo.LeagueTeamPlayerTransfers (SeasonId);
