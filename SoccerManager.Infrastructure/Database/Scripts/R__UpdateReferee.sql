CREATE OR ALTER PROCEDURE dbo.UpdateReferee
    @Id         INT,
    @Name       NVARCHAR(100),
    @ImageUrl   NVARCHAR(500),
    @Tolerance  TINYINT,
    @ModifiedAt DATETIME2(7),
    @ModifiedBy UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.Referees
    SET Name       = @Name,
        ImageUrl   = @ImageUrl,
        Tolerance  = @Tolerance,
        ModifiedAt = @ModifiedAt,
        ModifiedBy = @ModifiedBy
    WHERE Id = @Id;

    SELECT @Id;
END
