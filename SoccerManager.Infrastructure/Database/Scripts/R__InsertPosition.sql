CREATE OR ALTER PROCEDURE dbo.InsertPosition
    @Name       NVARCHAR(100),
    @Area       TINYINT,
    @Side       TINYINT,
    @CreatedAt  DATETIME2(7),
    @ModifiedAt DATETIME2(7),
    @CreatedBy  UNIQUEIDENTIFIER,
    @ModifiedBy UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.Positions (Name, Area, Side, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy)
    OUTPUT INSERTED.Id
    VALUES (@Name, @Area, @Side, @CreatedAt, @ModifiedAt, @CreatedBy, @ModifiedBy);
END
