CREATE OR ALTER PROCEDURE dbo.UpdatePosition
    @Id           INT,
    @Name         NVARCHAR(100),
    @ShortName    NVARCHAR(10),
    @CatalogOrder TINYINT,
    @Area         TINYINT,
    @Side         TINYINT,
    @ModifiedAt   DATETIME2(7),
    @ModifiedBy   UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.Positions
    SET Name         = @Name,
        ShortName    = @ShortName,
        CatalogOrder = @CatalogOrder,
        Area         = @Area,
        Side         = @Side,
        ModifiedAt   = @ModifiedAt,
        ModifiedBy   = @ModifiedBy
    WHERE Id = @Id;

    SELECT @Id;
END
