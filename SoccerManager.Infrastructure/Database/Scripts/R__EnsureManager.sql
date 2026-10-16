-- Returns the identifier of @UserId's manager profile, creating it first when there is none, so the UI can call it
-- after every login. UPDLOCK and HOLDLOCK keep the existence check's key range locked until the insert, inside the
-- caller's transaction (as dbo.DeleteLeagueTeam does), so two first logins at once create one row: the second waits,
-- then finds the first one's row instead of failing on UQ_Managers_UserId.
CREATE OR ALTER PROCEDURE dbo.EnsureManager
    @UserId     UNIQUEIDENTIFIER,
    @ImageUrl   NVARCHAR(500),
    @CreatedAt  DATETIME2(7),
    @ModifiedAt DATETIME2(7),
    @CreatedBy  UNIQUEIDENTIFIER,
    @ModifiedBy UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM dbo.Managers WITH (UPDLOCK, HOLDLOCK) WHERE UserId = @UserId)
    BEGIN
        INSERT INTO dbo.Managers (UserId, ImageUrl, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy)
        VALUES (@UserId, @ImageUrl, @CreatedAt, @ModifiedAt, @CreatedBy, @ModifiedBy);
    END

    SELECT Id
    FROM dbo.Managers
    WHERE UserId = @UserId;
END
