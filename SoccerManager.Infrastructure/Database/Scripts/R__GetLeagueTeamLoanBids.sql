-- The loan bids of one club, behind GET /api/league-teams/{id}/loan-bids, as one result set. It does not check access: the query
-- handler does that first, through IClubAccess, and there is no existence check, so an unknown @LeagueTeamId gives an empty set.
-- @LeagueTeamId is the club whose bids are listed, in both directions. A bid it PLACED has the club as borrower
-- (ToLeagueTeamId) and IsReceived = 0, the other club being the lender. A bid it RECEIVED is a loan on a squad row the club holds
-- now and IsReceived = 1, the other club being the borrower (ToLeagueTeamId).
-- dbo.LeagueTeamPlayerLoans has no lender column, so the lender is whoever holds the squad row now (dbo.LeagueTeamPlayers.
-- LeagueTeamId). A loan whose row already sits at the borrowing club has no known lender and is listed by neither branch: the
-- Generator step that completes loans must add a lender column first.
-- The loans are read once into @Bids, and the set is taken from it.
-- Statuses (TransferStatus): Pending (1), Accepted (2) and Completed (4) are listed, Rejected (3) never is. A new status needs a
-- decision here and in the UI, because the handler splits the rows into Pending and Completed (Accepted or Completed) by Status.
-- The set is ordered pending first, then newest first (CreatedAt DESC, Id DESC). LeagueTeamId, TeamName and TeamLogoUrl are the
-- OTHER club's, ManagerName is that club's current manager's Managers.FullName (UX_LeagueTeamManagers_LeagueTeamId_Current allows
-- at most one), NULL when the club has none or the manager has no name. SeasonNumber is the number of the loan's season within
-- its league. PlayerId and PlayerName are the player the bid is for and PlayerImageUrl is his photo, NULL when he has none.
-- The deal tables count separately, so a loan and a transfer can share an Id: the handler keeps the two lists apart.
CREATE OR ALTER PROCEDURE dbo.GetLeagueTeamLoanBids
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
        LoanFee DECIMAL(18,2) NOT NULL,
        SeasonsLength INT NOT NULL,
        BuyOptionValue DECIMAL(18,2) NULL,
        CreatedAt DATETIME2(7) NOT NULL);

    INSERT INTO @Bids (Id, IsReceived, Status, LeagueTeamPlayerId, OtherLeagueTeamId, SeasonId, LoanFee, SeasonsLength, BuyOptionValue, CreatedAt)
    SELECT l.Id,
           CAST(1 AS BIT),
           l.Status,
           l.LeagueTeamPlayerId,
           l.ToLeagueTeamId,
           l.SeasonId,
           l.LoanFee,
           l.SeasonsLength,
           l.BuyOptionValue,
           l.CreatedAt
    FROM dbo.LeagueTeamPlayerLoans l
        INNER JOIN dbo.LeagueTeamPlayers ltp ON ltp.Id = l.LeagueTeamPlayerId
    WHERE ltp.LeagueTeamId = @LeagueTeamId
      AND l.ToLeagueTeamId <> @LeagueTeamId
      AND l.Status IN (1, 2, 4)
    UNION ALL
    SELECT l.Id,
           CAST(0 AS BIT),
           l.Status,
           l.LeagueTeamPlayerId,
           ltp.LeagueTeamId,
           l.SeasonId,
           l.LoanFee,
           l.SeasonsLength,
           l.BuyOptionValue,
           l.CreatedAt
    FROM dbo.LeagueTeamPlayerLoans l
        INNER JOIN dbo.LeagueTeamPlayers ltp ON ltp.Id = l.LeagueTeamPlayerId
    WHERE l.ToLeagueTeamId = @LeagueTeamId
      AND ltp.LeagueTeamId <> @LeagueTeamId
      AND l.Status IN (1, 2, 4);

    SELECT b.Id,
           b.IsReceived,
           b.Status,
           p.Id AS PlayerId,
           p.Name AS PlayerName,
           p.ImageUrl AS PlayerImageUrl,
           b.OtherLeagueTeamId AS LeagueTeamId,
           tm.Name AS TeamName,
           tm.LogoUrl AS TeamLogoUrl,
           m.FullName AS ManagerName,
           s.SeasonNumber,
           b.LoanFee,
           b.SeasonsLength,
           b.BuyOptionValue,
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
END
