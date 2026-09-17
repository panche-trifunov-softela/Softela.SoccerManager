CREATE OR ALTER PROCEDURE dbo.UpdateManager
    @Id         INT,
    @ImageUrl   NVARCHAR(500),
    @ModifiedAt DATETIME2(7),
    @ModifiedBy UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.Managers
    SET ImageUrl   = @ImageUrl,
        ModifiedAt = @ModifiedAt,
        ModifiedBy = @ModifiedBy
    WHERE Id = @Id;

    SELECT @Id;
END
