CREATE OR ALTER PROCEDURE dbo.InsertManager
    @UserId     UNIQUEIDENTIFIER,
    @ImageUrl   NVARCHAR(500),
    @CreatedAt  DATETIME2(7),
    @ModifiedAt DATETIME2(7),
    @CreatedBy  UNIQUEIDENTIFIER,
    @ModifiedBy UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.Managers (UserId, ImageUrl, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy)
    OUTPUT INSERTED.Id
    VALUES (@UserId, @ImageUrl, @CreatedAt, @ModifiedAt, @CreatedBy, @ModifiedBy);
END
