CREATE OR ALTER PROCEDURE dbo.UpdateStadium
    @Id         INT,
    @Name       NVARCHAR(100),
    @ImageUrl   NVARCHAR(500),
    @Size       INT,
    @ModifiedAt DATETIME2(7),
    @ModifiedBy UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.Stadiums
    SET Name       = @Name,
        ImageUrl   = @ImageUrl,
        Size       = @Size,
        ModifiedAt = @ModifiedAt,
        ModifiedBy = @ModifiedBy
    WHERE Id = @Id;

    SELECT @Id;
END
