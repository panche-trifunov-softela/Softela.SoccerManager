CREATE OR ALTER PROCEDURE dbo.DeletePlayer
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    -- The player's squad rows, with their own transfers, loans and injuries, cascade with the player, but the swap rows that name the
    -- player as a swapped player cannot (the transfer already reaches them through a cascade), so they go first.
    DELETE sw
    FROM dbo.LeagueTeamPlayerTransferSwaps sw
    INNER JOIN dbo.LeagueTeamPlayers ltp ON ltp.Id = sw.LeagueTeamPlayerId
    WHERE ltp.PlayerId = @Id;

    -- This is a hard delete; the table has no soft-delete flag.
    DELETE FROM dbo.Players WHERE Id = @Id;
END
