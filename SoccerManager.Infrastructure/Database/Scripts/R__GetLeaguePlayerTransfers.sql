-- One player's accepted transfers and loans in a league, behind GET /api/league-teams/{id}/league-players/{playerId}/transfers,
-- as three result sets: whether the player is registered in the league, then the deals the player is part of, then the
-- players swapped in those deals. It does not check access: the query handler does that first, through IClubAccess.
-- The league is the league of @LeagueTeamId, found as in dbo.GetLeaguePlayerProfile. @PlayerRows holds the player's squad rows
-- (dbo.LeagueTeamPlayers) at every club of that league, rival clubs included, because a deal points at a squad row, not at a
-- player, and the player can hold several rows in a league over time.
-- The first result set is one row with one column, IsRegistered: 1 when @PlayerRows holds a row, else 0. An unknown
-- @LeagueTeamId gives 0. The handler answers a 0 with a 404, as the profile read does. The other two sets are returned whatever
-- the first one says.
-- The second result set is every ACCEPTED deal (Status 2 is TransferStatus.Accepted; a pending or rejected one is no history),
-- newest season first, then newest saved. One row says where the player joined: a sold player joined the buying club
-- (ToLeagueTeamId), a swapped player joined the selling club (FromLeagueTeamId) and a loaned player joined the loan club
-- (ToLeagueTeamId). IsLoan tells a loan from a transfer; their Ids come from two tables and can repeat, so the pair (IsLoan, Id)
-- is the key. Fee is the transfer fee or the loan fee. PlayerId and PlayerName are the player the deal is about, the sold or
-- the loaned one, so a swapped player's row names the player he was swapped for. TeamName and TeamLogoUrl are the joined club's
-- team's, as in the profile.
-- The third result set is the swapped players of every accepted transfer in the second one, in swap-row order, so the handler
-- can list them under the transfer that carried them.
CREATE OR ALTER PROCEDURE dbo.GetLeaguePlayerTransfers
    @LeagueTeamId INT,
    @PlayerId INT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @PlayerRows TABLE (Id INT NOT NULL PRIMARY KEY);

    INSERT INTO @PlayerRows (Id)
    SELECT ltp.Id
    FROM dbo.LeagueTeams anchor
        INNER JOIN dbo.LeagueTeams lt ON lt.LeagueId = anchor.LeagueId
        INNER JOIN dbo.LeagueTeamPlayers ltp ON ltp.LeagueTeamId = lt.Id AND ltp.PlayerId = @PlayerId
    WHERE anchor.Id = @LeagueTeamId;

    SELECT CAST(CASE WHEN EXISTS (SELECT 1 FROM @PlayerRows) THEN 1 ELSE 0 END AS BIT) AS IsRegistered;

    SELECT d.Id,
           d.IsLoan,
           d.LeagueTeamId,
           t.Name AS TeamName,
           t.LogoUrl AS TeamLogoUrl,
           d.SeasonId,
           s.SeasonNumber,
           d.Fee,
           p.Id AS PlayerId,
           p.Name AS PlayerName
    FROM (
        SELECT tr.Id,
               CAST(0 AS BIT) AS IsLoan,
               tr.ToLeagueTeamId AS LeagueTeamId,
               tr.SeasonId,
               tr.TransferFee AS Fee,
               tr.LeagueTeamPlayerId AS DealLeagueTeamPlayerId,
               tr.CreatedAt
        FROM dbo.LeagueTeamPlayerTransfers tr
        WHERE tr.Status = 2
          AND tr.LeagueTeamPlayerId IN (SELECT Id FROM @PlayerRows)

        UNION ALL

        SELECT tr.Id,
               CAST(0 AS BIT),
               tr.FromLeagueTeamId,
               tr.SeasonId,
               tr.TransferFee,
               tr.LeagueTeamPlayerId,
               tr.CreatedAt
        FROM dbo.LeagueTeamPlayerTransferSwaps sw
            INNER JOIN dbo.LeagueTeamPlayerTransfers tr ON tr.Id = sw.LeagueTeamPlayerTransferId
        WHERE tr.Status = 2
          AND sw.LeagueTeamPlayerId IN (SELECT Id FROM @PlayerRows)

        UNION ALL

        SELECT lo.Id,
               CAST(1 AS BIT),
               lo.ToLeagueTeamId,
               lo.SeasonId,
               lo.LoanFee,
               lo.LeagueTeamPlayerId,
               lo.CreatedAt
        FROM dbo.LeagueTeamPlayerLoans lo
        WHERE lo.Status = 2
          AND lo.LeagueTeamPlayerId IN (SELECT Id FROM @PlayerRows)
    ) d
        INNER JOIN dbo.LeagueTeams jlt ON jlt.Id = d.LeagueTeamId
        INNER JOIN dbo.Teams t ON t.Id = jlt.TeamId
        INNER JOIN dbo.Seasons s ON s.Id = d.SeasonId
        INNER JOIN dbo.LeagueTeamPlayers dltp ON dltp.Id = d.DealLeagueTeamPlayerId
        INNER JOIN dbo.Players p ON p.Id = dltp.PlayerId
    ORDER BY s.SeasonNumber DESC,
             d.CreatedAt DESC,
             d.Id DESC;

    SELECT sw.LeagueTeamPlayerTransferId,
           p.Id AS PlayerId,
           p.Name
    FROM dbo.LeagueTeamPlayerTransferSwaps sw
        INNER JOIN dbo.LeagueTeamPlayerTransfers tr ON tr.Id = sw.LeagueTeamPlayerTransferId
        INNER JOIN dbo.LeagueTeamPlayers ltp ON ltp.Id = sw.LeagueTeamPlayerId
        INNER JOIN dbo.Players p ON p.Id = ltp.PlayerId
    WHERE tr.Status = 2
      AND (tr.LeagueTeamPlayerId IN (SELECT Id FROM @PlayerRows)
           OR EXISTS (SELECT 1
                      FROM dbo.LeagueTeamPlayerTransferSwaps own
                      WHERE own.LeagueTeamPlayerTransferId = tr.Id
                        AND own.LeagueTeamPlayerId IN (SELECT Id FROM @PlayerRows)))
    ORDER BY sw.LeagueTeamPlayerTransferId,
             sw.Id;
END
