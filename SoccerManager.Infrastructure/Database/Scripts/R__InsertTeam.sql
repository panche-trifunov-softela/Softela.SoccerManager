CREATE OR ALTER PROCEDURE dbo.InsertTeam
    @Name            NVARCHAR(100),
    @StadiumId       INT,
    @FinancialState  TINYINT,
    @JerseyUrl       NVARCHAR(500),
    @LogoUrl         NVARCHAR(500),
    @TransfermarktId INT,
    @CreatedAt       DATETIME2(7),
    @ModifiedAt      DATETIME2(7),
    @CreatedBy       UNIQUEIDENTIFIER,
    @ModifiedBy      UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.Teams (Name, StadiumId, FinancialState, JerseyUrl, LogoUrl, TransfermarktId, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy)
    OUTPUT INSERTED.Id
    VALUES (@Name, @StadiumId, @FinancialState, @JerseyUrl, @LogoUrl, @TransfermarktId, @CreatedAt, @ModifiedAt, @CreatedBy, @ModifiedBy);
END
