CREATE OR ALTER PROCEDURE dbo.UpdateFormation
    @Id         INT,
    @Name       NVARCHAR(100),
    @ModifiedAt DATETIME2(7),
    @ModifiedBy UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.Formations
    SET Name       = @Name,
        ModifiedAt = @ModifiedAt,
        ModifiedBy = @ModifiedBy
    WHERE Id = @Id;

    SELECT @Id;
END
