CREATE OR ALTER PROCEDURE dbo.InsertNationalTeam
    @Name            NVARCHAR(100),
    @StadiumId       INT,
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

    INSERT INTO dbo.NationalTeams (Name, StadiumId, JerseyUrl, LogoUrl, TransfermarktId, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy)
    OUTPUT INSERTED.Id
    VALUES (@Name, @StadiumId, @JerseyUrl, @LogoUrl, @TransfermarktId, @CreatedAt, @ModifiedAt, @CreatedBy, @ModifiedBy);
END
