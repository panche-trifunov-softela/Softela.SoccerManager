CREATE OR ALTER PROCEDURE dbo.DeleteLeagueTeamPlayer
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    -- The player's own transfers, loans and injuries cascade with the squad row, but the swap rows that name the player as a swapped
    -- player cannot (the transfer already reaches them through a cascade), so they go first.
    DELETE FROM dbo.LeagueTeamPlayerTransferSwaps WHERE LeagueTeamPlayerId = @Id;

    -- This is a hard delete; the table has no soft-delete flag.
    DELETE FROM dbo.LeagueTeamPlayers WHERE Id = @Id;
END
