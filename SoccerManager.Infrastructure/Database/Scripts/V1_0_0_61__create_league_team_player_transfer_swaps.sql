-- dbo.LeagueTeamPlayerTransferSwaps holds one row per player of the buying club who goes to the selling club as part of a transfer; LeagueTeamPlayerId is that player's squad row at the buying club.
-- FK_LeagueTeamPlayerTransferSwaps_LeagueTeamPlayerTransfers cascades: deleting a transfer removes its swap rows through a single path.
-- FK_LeagueTeamPlayerTransferSwaps_LeagueTeamPlayers does not cascade: LeagueTeamPlayers already reaches this table through dbo.LeagueTeamPlayerTransfers, and a second path would be rejected (Msg 1785). dbo.DeleteLeagueTeamPlayer and dbo.DeletePlayer therefore delete a player's swap rows first.
-- UQ_LeagueTeamPlayerTransferSwaps_LeagueTeamPlayerTransferId_LeagueTeamPlayerId lists a player once per transfer and also backs the by-transfer lookup.
-- IX_LeagueTeamPlayerTransferSwaps_LeagueTeamPlayerId backs the foreign key and the deletes by squad row.
-- That the swapped player is in the buying club's squad, and is not the sold player, is checked by the API when the row is saved; nothing in the database checks it.
CREATE TABLE dbo.LeagueTeamPlayerTransferSwaps (
    Id                          INT              IDENTITY(1,1)    NOT NULL,
    LeagueTeamPlayerTransferId  INT                               NOT NULL,
    LeagueTeamPlayerId          INT                               NOT NULL,
    CreatedAt                   DATETIME2(7)                      NOT NULL,
    ModifiedAt                  DATETIME2(7)                      NOT NULL,
    CreatedBy                   UNIQUEIDENTIFIER                  NOT NULL,
    ModifiedBy                  UNIQUEIDENTIFIER                  NOT NULL,
    CONSTRAINT PK_LeagueTeamPlayerTransferSwaps PRIMARY KEY CLUSTERED (Id),
    CONSTRAINT FK_LeagueTeamPlayerTransferSwaps_LeagueTeamPlayerTransfers FOREIGN KEY (LeagueTeamPlayerTransferId) REFERENCES dbo.LeagueTeamPlayerTransfers (Id) ON DELETE CASCADE,
    CONSTRAINT FK_LeagueTeamPlayerTransferSwaps_LeagueTeamPlayers FOREIGN KEY (LeagueTeamPlayerId) REFERENCES dbo.LeagueTeamPlayers (Id),
    CONSTRAINT UQ_LeagueTeamPlayerTransferSwaps_LeagueTeamPlayerTransferId_LeagueTeamPlayerId UNIQUE (LeagueTeamPlayerTransferId, LeagueTeamPlayerId)
);

CREATE NONCLUSTERED INDEX IX_LeagueTeamPlayerTransferSwaps_LeagueTeamPlayerId ON dbo.LeagueTeamPlayerTransferSwaps (LeagueTeamPlayerId);
