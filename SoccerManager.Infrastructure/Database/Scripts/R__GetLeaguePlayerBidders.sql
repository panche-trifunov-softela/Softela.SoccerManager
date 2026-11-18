-- The clubs that have bid for one player of a league, behind GET /api/league-teams/{id}/league-players/{playerId}/bidders, as two
-- result sets: whether the player is registered in the league, then the bids. It does not check access: the query handler does
-- that first, through IClubAccess.
-- The league is the league of @LeagueTeamId, found as in dbo.GetLeaguePlayerTransfers. @PlayerRows holds the player's squad rows
-- (dbo.LeagueTeamPlayers) at every club of that league, rival clubs included, because any manager of the league may ask who has
-- bid for a player, whichever club holds him, and a bid points at a squad row, not at a player. Each row keeps the LeagueTeamId
-- of the club it is at.
-- The first result set is one row with one column, IsRegistered: 1 when @PlayerRows holds a row, else 0. An unknown
-- @LeagueTeamId gives 0. The handler answers a 0 with a 404, as dbo.GetLeaguePlayerTransfers does. The second set is returned
-- whatever the first one says.
-- The second result set is every bid on a squad row in @PlayerRows that is still open: Status 1 is TransferStatus.Pending and
-- Status 2 is TransferStatus.Accepted, a deal agreed but not completed yet. Nothing completes a deal yet; the Generator step
-- that moves the players later must mark a completed deal, for example with a Completed status, so that it drops out of this
-- list. A rejected bid (Status 3) is not listed. A transfer counts only while its seller (FromLeagueTeamId) is the club the squad
-- row is at, the same filter as the holder's own list in dbo.GetLeaguePlayerBids, so a bid left over from a club the player has
-- since left is not listed. dbo.LeagueTeamPlayerLoans has no seller column, so the squad row alone says whose player it is.
-- LeagueTeamId, TeamName and TeamLogoUrl are the BIDDING club's (ToLeagueTeamId). ManagerName is that club's current manager's
-- Managers.FullName (UX_LeagueTeamManagers_LeagueTeamId_Current allows at most one), NULL when the club has none or the manager
-- has no name. SeasonNumber is the number of the bid's season within its league. IsLoan tells a loan from a transfer; their Ids
-- come from two tables and can repeat, so the pair (IsLoan, Id) is the key.
-- No fee or other term of a bid is returned, on purpose: any manager of the league may read this list, and the terms are for the
-- holding club alone, through dbo.GetLeaguePlayerBids.
-- The order is accepted deals first, then newest first (CreatedAt DESC), then a transfer before a loan, then the higher Id first.
CREATE OR ALTER PROCEDURE dbo.GetLeaguePlayerBidders
    @LeagueTeamId INT,
    @PlayerId INT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @PlayerRows TABLE (Id INT NOT NULL PRIMARY KEY, LeagueTeamId INT NOT NULL);

    INSERT INTO @PlayerRows (Id, LeagueTeamId)
    SELECT ltp.Id,
           ltp.LeagueTeamId
    FROM dbo.LeagueTeams anchor
        INNER JOIN dbo.LeagueTeams lt ON lt.LeagueId = anchor.LeagueId
        INNER JOIN dbo.LeagueTeamPlayers ltp ON ltp.LeagueTeamId = lt.Id AND ltp.PlayerId = @PlayerId
    WHERE anchor.Id = @LeagueTeamId;

    SELECT CAST(CASE WHEN EXISTS (SELECT 1 FROM @PlayerRows) THEN 1 ELSE 0 END AS BIT) AS IsRegistered;

    SELECT b.Id,
           b.IsLoan,
           b.Status,
           b.LeagueTeamId,
           tm.Name AS TeamName,
           tm.LogoUrl AS TeamLogoUrl,
           m.FullName AS ManagerName,
           s.SeasonNumber,
           b.CreatedAt
    FROM (
        SELECT t.Id,
               CAST(0 AS BIT) AS IsLoan,
               t.Status,
               t.ToLeagueTeamId AS LeagueTeamId,
               t.SeasonId,
               t.CreatedAt
        FROM dbo.LeagueTeamPlayerTransfers t
            INNER JOIN @PlayerRows pr ON pr.Id = t.LeagueTeamPlayerId AND pr.LeagueTeamId = t.FromLeagueTeamId
        WHERE t.Status IN (1, 2)

        UNION ALL

        SELECT l.Id,
               CAST(1 AS BIT),
               l.Status,
               l.ToLeagueTeamId,
               l.SeasonId,
               l.CreatedAt
        FROM dbo.LeagueTeamPlayerLoans l
            INNER JOIN @PlayerRows pr ON pr.Id = l.LeagueTeamPlayerId
        WHERE l.Status IN (1, 2)
    ) b
        INNER JOIN dbo.LeagueTeams blt ON blt.Id = b.LeagueTeamId
        INNER JOIN dbo.Teams tm ON tm.Id = blt.TeamId
        INNER JOIN dbo.Seasons s ON s.Id = b.SeasonId
        LEFT JOIN dbo.LeagueTeamManagers ltm ON ltm.LeagueTeamId = b.LeagueTeamId AND ltm.IsCurrent = 1
        LEFT JOIN dbo.Managers m ON m.Id = ltm.ManagerId
    ORDER BY b.Status DESC,
             b.CreatedAt DESC,
             b.IsLoan,
             b.Id DESC;
END
