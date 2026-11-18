-- The pending bids on one player of a club, behind GET /api/league-teams/{id}/league-players/{playerId}/bids, as four result
-- sets: whether the player is registered at the club, then the transfer bids, then the players swapped in those bids, then the
-- loan bids. It does not check access: the query handler does that first, through IClubAccess.
-- @LeagueTeamId is the club that holds the player, the one answering the bids. @PlayerRows holds the player's squad rows
-- (dbo.LeagueTeamPlayers) at that club only, because a bid points at a squad row, not at a player.
-- The first result set is one row with one column, IsRegistered: 1 when @PlayerRows holds a row, else 0. An unknown
-- @LeagueTeamId gives 0. The handler answers a 0 with a 404, as dbo.GetLeaguePlayerTransfers does. The other sets are returned
-- whatever the first one says.
-- The second result set is every PENDING transfer bid (Status 1 is TransferStatus.Pending) on a squad row in @PlayerRows whose
-- seller (FromLeagueTeamId) is @LeagueTeamId, newest first (Id DESC). The transfers are read once into @TransferBids, and the
-- rows of this set and of the third one are both taken from it, so a bid answered in between cannot show up in one set and not
-- in the other. LeagueTeamId, TeamName and TeamLogoUrl are the BIDDING club's (ToLeagueTeamId). ManagerName is that club's
-- current manager's Managers.FullName (UX_LeagueTeamManagers_LeagueTeamId_Current allows at most one), NULL when the club has
-- none or the manager has no name. SeasonNumber is the number of the bid's season within its league.
-- The third result set is the players swapped in those bids, in swap-row order, so the handler can list them under the bid that
-- carries them. PositionShortName is the main position's short name and Rating the current rating (the dbo.PlayerRatings row
-- with IsCurrent = 1), both as in dbo.GetLeagueTeamSquadMembers; a player without a current rating gets NULL.
-- The fourth result set is every PENDING loan bid on a squad row in @PlayerRows, newest first, with the same bidding-club
-- columns. dbo.LeagueTeamPlayerLoans has no seller column, so the squad row alone says whose player it is.
-- The deal tables count separately, so a transfer and a loan can share an Id: the handler keeps the two lists apart.
CREATE OR ALTER PROCEDURE dbo.GetLeaguePlayerBids
    @LeagueTeamId INT,
    @PlayerId INT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @PlayerRows TABLE (Id INT NOT NULL PRIMARY KEY);

    INSERT INTO @PlayerRows (Id)
    SELECT ltp.Id
    FROM dbo.LeagueTeamPlayers ltp
    WHERE ltp.LeagueTeamId = @LeagueTeamId
      AND ltp.PlayerId = @PlayerId;

    DECLARE @TransferBids TABLE (
        Id INT NOT NULL PRIMARY KEY,
        LeagueTeamId INT NOT NULL,
        SeasonId INT NOT NULL,
        TransferFee DECIMAL(18,2) NOT NULL,
        BuybackValue DECIMAL(18,2) NULL,
        BuybackValidSeasons INT NULL,
        CreatedAt DATETIME2(7) NOT NULL);

    INSERT INTO @TransferBids (Id, LeagueTeamId, SeasonId, TransferFee, BuybackValue, BuybackValidSeasons, CreatedAt)
    SELECT t.Id,
           t.ToLeagueTeamId,
           t.SeasonId,
           t.TransferFee,
           t.BuybackValue,
           t.BuybackValidSeasons,
           t.CreatedAt
    FROM dbo.LeagueTeamPlayerTransfers t
    WHERE t.Status = 1
      AND t.FromLeagueTeamId = @LeagueTeamId
      AND t.LeagueTeamPlayerId IN (SELECT Id FROM @PlayerRows);

    SELECT CAST(CASE WHEN EXISTS (SELECT 1 FROM @PlayerRows) THEN 1 ELSE 0 END AS BIT) AS IsRegistered;

    SELECT b.Id,
           b.LeagueTeamId,
           tm.Name AS TeamName,
           tm.LogoUrl AS TeamLogoUrl,
           m.FullName AS ManagerName,
           s.SeasonNumber,
           b.TransferFee,
           b.BuybackValue,
           b.BuybackValidSeasons,
           b.CreatedAt
    FROM @TransferBids b
        INNER JOIN dbo.LeagueTeams blt ON blt.Id = b.LeagueTeamId
        INNER JOIN dbo.Teams tm ON tm.Id = blt.TeamId
        INNER JOIN dbo.Seasons s ON s.Id = b.SeasonId
        LEFT JOIN dbo.LeagueTeamManagers ltm ON ltm.LeagueTeamId = b.LeagueTeamId AND ltm.IsCurrent = 1
        LEFT JOIN dbo.Managers m ON m.Id = ltm.ManagerId
    ORDER BY b.Id DESC;

    SELECT sw.LeagueTeamPlayerTransferId,
           p.Id AS PlayerId,
           p.Name,
           pos.ShortName AS PositionShortName,
           cr.Rating
    FROM dbo.LeagueTeamPlayerTransferSwaps sw
        INNER JOIN @TransferBids b ON b.Id = sw.LeagueTeamPlayerTransferId
        INNER JOIN dbo.LeagueTeamPlayers ltp ON ltp.Id = sw.LeagueTeamPlayerId
        INNER JOIN dbo.Players p ON p.Id = ltp.PlayerId
        LEFT JOIN dbo.PlayerRatings cr ON cr.PlayerId = p.Id AND cr.IsCurrent = 1
        OUTER APPLY dbo.GetPlayerMainPosition(p.Id) mp
        LEFT JOIN dbo.Positions pos ON pos.Id = mp.PositionId
    ORDER BY sw.LeagueTeamPlayerTransferId,
             sw.Id;

    SELECT l.Id,
           l.ToLeagueTeamId AS LeagueTeamId,
           tm.Name AS TeamName,
           tm.LogoUrl AS TeamLogoUrl,
           m.FullName AS ManagerName,
           s.SeasonNumber,
           l.LoanFee,
           l.SeasonsLength,
           l.BuyOptionValue,
           l.CreatedAt
    FROM dbo.LeagueTeamPlayerLoans l
        INNER JOIN dbo.LeagueTeams blt ON blt.Id = l.ToLeagueTeamId
        INNER JOIN dbo.Teams tm ON tm.Id = blt.TeamId
        INNER JOIN dbo.Seasons s ON s.Id = l.SeasonId
        LEFT JOIN dbo.LeagueTeamManagers ltm ON ltm.LeagueTeamId = l.ToLeagueTeamId AND ltm.IsCurrent = 1
        LEFT JOIN dbo.Managers m ON m.Id = ltm.ManagerId
    WHERE l.Status = 1
      AND l.LeagueTeamPlayerId IN (SELECT Id FROM @PlayerRows)
    ORDER BY l.Id DESC;
END
