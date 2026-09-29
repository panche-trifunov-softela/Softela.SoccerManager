CREATE OR ALTER PROCEDURE dbo.InsertReferee
    @Name       NVARCHAR(100),
    @ImageUrl   NVARCHAR(500),
    @Tolerance  TINYINT,
    @CreatedAt  DATETIME2(7),
    @ModifiedAt DATETIME2(7),
    @CreatedBy  UNIQUEIDENTIFIER,
    @ModifiedBy UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.Referees (Name, ImageUrl, Tolerance, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy)
    OUTPUT INSERTED.Id
    VALUES (@Name, @ImageUrl, @Tolerance, @CreatedAt, @ModifiedAt, @CreatedBy, @ModifiedBy);
END
