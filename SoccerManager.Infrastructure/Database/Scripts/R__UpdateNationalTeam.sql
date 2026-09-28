CREATE OR ALTER PROCEDURE dbo.UpdateNationalTeam
    @Id         INT,
    @Name       NVARCHAR(100),
    @StadiumId  INT,
    @JerseyUrl  NVARCHAR(500),
    @LogoUrl    NVARCHAR(500),
    @ModifiedAt DATETIME2(7),
    @ModifiedBy UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.NationalTeams
    SET Name       = @Name,
        StadiumId  = @StadiumId,
        JerseyUrl  = @JerseyUrl,
        LogoUrl    = @LogoUrl,
        ModifiedAt = @ModifiedAt,
        ModifiedBy = @ModifiedBy
    WHERE Id = @Id;

    SELECT @Id;
END
