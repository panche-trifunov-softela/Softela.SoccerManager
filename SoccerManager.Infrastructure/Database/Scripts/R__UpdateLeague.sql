CREATE OR ALTER PROCEDURE dbo.UpdateLeague
    @Id         INT,
    @Name       NVARCHAR(100),
    @ModifiedAt DATETIME2(7),
    @ModifiedBy UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.Leagues
    SET Name       = @Name,
        ModifiedAt = @ModifiedAt,
        ModifiedBy = @ModifiedBy
    WHERE Id = @Id;

    SELECT @Id;
END
