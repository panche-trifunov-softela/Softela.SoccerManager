CREATE OR ALTER PROCEDURE dbo.DeleteSeason
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    -- A season is removed only when no transfer or loan uses it. The checks take range locks
    -- (UPDLOCK, HOLDLOCK) inside the caller's transaction, so nothing can be attached between the check
    -- and the delete.
    DECLARE @Exists BIT = CASE WHEN EXISTS (
        SELECT 1 FROM dbo.Seasons WITH (UPDLOCK, HOLDLOCK) WHERE Id = @Id) THEN 1 ELSE 0 END;

    DECLARE @HasTransfers BIT = CASE WHEN EXISTS (
        SELECT 1 FROM dbo.LeagueTeamPlayerTransfers WITH (UPDLOCK, HOLDLOCK) WHERE SeasonId = @Id)
        OR EXISTS (
        SELECT 1 FROM dbo.LeagueTeamPlayerLoans WITH (UPDLOCK, HOLDLOCK) WHERE SeasonId = @Id) THEN 1 ELSE 0 END;

    DECLARE @Deleted BIT = 0;

    IF @Exists = 1 AND @HasTransfers = 0
    BEGIN
        -- This is a hard delete; the table has no soft-delete flag.
        DELETE FROM dbo.Seasons WHERE Id = @Id;

        SET @Deleted = 1;
    END

    SELECT @Deleted AS Deleted, @HasTransfers AS HasTransfers;
END
