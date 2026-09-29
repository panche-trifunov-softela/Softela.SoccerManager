CREATE OR ALTER PROCEDURE dbo.UpdatePlayer
    @Id             INT,
    @Name           NVARCHAR(100),
    @DateOfBirth    DATE,
    @Rating         INT,
    @Value          DECIMAL(18,2),
    @Wage           DECIMAL(18,2),
    @ImageUrl       NVARCHAR(500),
    @NationalTeamId INT,
    @TeamId         INT,
    @ModifiedAt     DATETIME2(7),
    @ModifiedBy     UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.Players
    SET Name           = @Name,
        DateOfBirth    = @DateOfBirth,
        Rating         = @Rating,
        [Value]        = @Value,
        Wage           = @Wage,
        ImageUrl       = @ImageUrl,
        NationalTeamId = @NationalTeamId,
        TeamId         = @TeamId,
        ModifiedAt     = @ModifiedAt,
        ModifiedBy     = @ModifiedBy
    WHERE Id = @Id;

    SELECT @Id;
END
