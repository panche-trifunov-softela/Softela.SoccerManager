CREATE OR ALTER PROCEDURE dbo.InsertPlayer
    @Name            NVARCHAR(100),
    @DateOfBirth     DATE,
    @Rating          INT,
    @Value           DECIMAL(18,2),
    @Wage            DECIMAL(18,2),
    @ImageUrl        NVARCHAR(500),
    @NationalTeamId  INT,
    @TeamId          INT,
    @TransfermarktId INT,
    @CreatedAt       DATETIME2(7),
    @ModifiedAt      DATETIME2(7),
    @CreatedBy       UNIQUEIDENTIFIER,
    @ModifiedBy      UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.Players (Name, DateOfBirth, Rating, [Value], Wage, ImageUrl, NationalTeamId, TeamId, TransfermarktId, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy)
    OUTPUT INSERTED.Id
    VALUES (@Name, @DateOfBirth, @Rating, @Value, @Wage, @ImageUrl, @NationalTeamId, @TeamId, @TransfermarktId, @CreatedAt, @ModifiedAt, @CreatedBy, @ModifiedBy);
END
