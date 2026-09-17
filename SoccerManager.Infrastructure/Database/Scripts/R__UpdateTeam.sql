CREATE OR ALTER PROCEDURE dbo.UpdateTeam
    @Id             INT,
    @Name           NVARCHAR(100),
    @StadiumId      INT,
    @FinancialState TINYINT,
    @JerseyUrl      NVARCHAR(500),
    @LogoUrl        NVARCHAR(500),
    @ModifiedAt     DATETIME2(7),
    @ModifiedBy     UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.Teams
    SET Name           = @Name,
        StadiumId      = @StadiumId,
        FinancialState = @FinancialState,
        JerseyUrl      = @JerseyUrl,
        LogoUrl        = @LogoUrl,
        ModifiedAt     = @ModifiedAt,
        ModifiedBy     = @ModifiedBy
    WHERE Id = @Id;

    SELECT @Id;
END
