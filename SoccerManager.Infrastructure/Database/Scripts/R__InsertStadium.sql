CREATE OR ALTER PROCEDURE dbo.InsertStadium
    @Name       NVARCHAR(100),
    @ImageUrl   NVARCHAR(500),
    @Size       INT,
    @CreatedAt  DATETIME2(7),
    @ModifiedAt DATETIME2(7),
    @CreatedBy  UNIQUEIDENTIFIER,
    @ModifiedBy UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.Stadiums (Name, ImageUrl, Size, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy)
    OUTPUT INSERTED.Id
    VALUES (@Name, @ImageUrl, @Size, @CreatedAt, @ModifiedAt, @CreatedBy, @ModifiedBy);
END
