-- The transfer bids of one club, behind GET /api/league-teams/{id}/transfer-bids, as two result sets: the bids, then the players
-- swapped in those bids. It does not check access: the query handler does that first, through IClubAccess, and there is no
-- existence check, so an unknown @LeagueTeamId gives two empty sets.
-- @LeagueTeamId is the club whose bids are listed, in both directions. A bid it PLACED has the club as buyer
-- (ToLeagueTeamId) and IsReceived = 0, the other club being the seller (FromLeagueTeamId). A bid it RECEIVED has the club as
-- seller (FromLeagueTeamId) and IsReceived = 1, the other club being the bidder (ToLeagueTeamId). The transfers are read once
-- into @Bids, and the rows of both sets are taken from it, so a bid answered in between cannot show up in one set and not in the
-- other.
-- Statuses (TransferStatus): Pending (1), Accepted (2) and Completed (4) are listed, Rejected (3) never is. A new status needs a
-- decision here and in the UI, because the handler splits the rows into Pending and Completed (Accepted or Completed) by Status.
-- A received PENDING bid counts only while the player is still at the club (the squad row's LeagueTeamId is @LeagueTeamId), the
-- same rule as dbo.GetLeaguePlayerBids, because only then can the club answer it. A received Accepted or Completed bid stays the
-- seller's history wherever the player is now. A placed bid is listed whatever became of the player.
-- The first result set is the bids, pending first, then newest first (CreatedAt DESC, Id DESC). LeagueTeamId, TeamName and
-- TeamLogoUrl are the OTHER club's, ManagerName is that club's current manager's Managers.FullName
-- (UX_LeagueTeamManagers_LeagueTeamId_Current allows at most one), NULL when the club has none or the manager has no name.
-- SeasonNumber is the number of the bid's season within its league. PlayerId and PlayerName are the player the bid is for.
-- The second result set is the players swapped in those bids, in swap-row order, so the handler can list them under the bid that
-- carries them. PositionShortName is the main position's short name and Rating the current rating (the dbo.PlayerRatings row
-- with IsCurrent = 1), both as in dbo.GetLeagueTeamSquadMembers; a player without a current rating gets NULL.
CREATE OR ALTER PROCEDURE dbo.GetLeagueTeamTransferBids
    @LeagueTeamId INT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Bids TABLE (
        Id INT NOT NULL PRIMARY KEY,
        IsReceived BIT NOT NULL,
        Status TINYINT NOT NULL,
        LeagueTeamPlayerId INT NOT NULL,
        OtherLeagueTeamId INT NOT NULL,
        SeasonId INT NOT NULL,
        TransferFee DECIMAL(18,2) NOT NULL,
        BuybackValue DECIMAL(18,2) NULL,
        BuybackValidSeasons INT NULL,
        CreatedAt DATETIME2(7) NOT NULL);

    INSERT INTO @Bids (Id, IsReceived, Status, LeagueTeamPlayerId, OtherLeagueTeamId, SeasonId, TransferFee, BuybackValue, BuybackValidSeasons, CreatedAt)
    SELECT t.Id,
           CAST(1 AS BIT),
           t.Status,
           t.LeagueTeamPlayerId,
           t.ToLeagueTeamId,
           t.SeasonId,
           t.TransferFee,
           t.BuybackValue,
           t.BuybackValidSeasons,
           t.CreatedAt
    FROM dbo.LeagueTeamPlayerTransfers t
        INNER JOIN dbo.LeagueTeamPlayers ltp ON ltp.Id = t.LeagueTeamPlayerId
    WHERE t.FromLeagueTeamId = @LeagueTeamId
      AND (t.Status IN (2, 4) OR (t.Status = 1 AND ltp.LeagueTeamId = @LeagueTeamId))
    UNION ALL
    SELECT t.Id,
           CAST(0 AS BIT),
           t.Status,
           t.LeagueTeamPlayerId,
           t.FromLeagueTeamId,
           t.SeasonId,
           t.TransferFee,
           t.BuybackValue,
           t.BuybackValidSeasons,
           t.CreatedAt
    FROM dbo.LeagueTeamPlayerTransfers t
    WHERE t.ToLeagueTeamId = @LeagueTeamId
      AND t.Status IN (1, 2, 4);

    SELECT b.Id,
           b.IsReceived,
           b.Status,
           p.Id AS PlayerId,
           p.Name AS PlayerName,
           b.OtherLeagueTeamId AS LeagueTeamId,
           tm.Name AS TeamName,
           tm.LogoUrl AS TeamLogoUrl,
           m.FullName AS ManagerName,
           s.SeasonNumber,
           b.TransferFee,
           b.BuybackValue,
           b.BuybackValidSeasons,
           b.CreatedAt
    FROM @Bids b
        INNER JOIN dbo.LeagueTeamPlayers ltp ON ltp.Id = b.LeagueTeamPlayerId
        INNER JOIN dbo.Players p ON p.Id = ltp.PlayerId
        INNER JOIN dbo.LeagueTeams blt ON blt.Id = b.OtherLeagueTeamId
        INNER JOIN dbo.Teams tm ON tm.Id = blt.TeamId
        INNER JOIN dbo.Seasons s ON s.Id = b.SeasonId
        LEFT JOIN dbo.LeagueTeamManagers ltm ON ltm.LeagueTeamId = b.OtherLeagueTeamId AND ltm.IsCurrent = 1
        LEFT JOIN dbo.Managers m ON m.Id = ltm.ManagerId
    ORDER BY CASE WHEN b.Status = 1 THEN 0 ELSE 1 END,
             b.CreatedAt DESC,
             b.Id DESC;

    SELECT sw.LeagueTeamPlayerTransferId,
           p.Id AS PlayerId,
           p.Name,
           pos.ShortName AS PositionShortName,
           cr.Rating
    FROM dbo.LeagueTeamPlayerTransferSwaps sw
        INNER JOIN @Bids b ON b.Id = sw.LeagueTeamPlayerTransferId
        INNER JOIN dbo.LeagueTeamPlayers ltp ON ltp.Id = sw.LeagueTeamPlayerId
        INNER JOIN dbo.Players p ON p.Id = ltp.PlayerId
        LEFT JOIN dbo.PlayerRatings cr ON cr.PlayerId = p.Id AND cr.IsCurrent = 1
        OUTER APPLY dbo.GetPlayerMainPosition(p.Id) mp
        LEFT JOIN dbo.Positions pos ON pos.Id = mp.PositionId
    ORDER BY sw.LeagueTeamPlayerTransferId,
             sw.Id;
END
