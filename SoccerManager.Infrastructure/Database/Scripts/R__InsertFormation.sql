CREATE OR ALTER PROCEDURE dbo.InsertFormation
    @Name       NVARCHAR(100),
    @CreatedAt  DATETIME2(7),
    @ModifiedAt DATETIME2(7),
    @CreatedBy  UNIQUEIDENTIFIER,
    @ModifiedBy UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.Formations (Name, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy)
    OUTPUT INSERTED.Id
    VALUES (@Name, @CreatedAt, @ModifiedAt, @CreatedBy, @ModifiedBy);
END
