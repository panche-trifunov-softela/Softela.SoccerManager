CREATE OR ALTER PROCEDURE dbo.UpdateCompetition
    @Id              INT,
    @Name            NVARCHAR(100),
    @LogoUrl         NVARCHAR(500),
    @IsDomestic      BIT,
    @Format          TINYINT,
    @MaxAgeAllowed   INT,
    @Order           INT,
    @TeamsPromoted   INT,
    @TeamsRelegated  INT,
    @TeamsInPlayoffs INT,
    @ModifiedAt      DATETIME2(7),
    @ModifiedBy      UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    -- LeagueId is fixed at creation, so a competition never moves between leagues.
    UPDATE dbo.Competitions
    SET Name            = @Name,
        LogoUrl         = @LogoUrl,
        IsDomestic      = @IsDomestic,
        Format          = @Format,
        MaxAgeAllowed   = @MaxAgeAllowed,
        [Order]         = @Order,
        TeamsPromoted   = @TeamsPromoted,
        TeamsRelegated  = @TeamsRelegated,
        TeamsInPlayoffs = @TeamsInPlayoffs,
        ModifiedAt      = @ModifiedAt,
        ModifiedBy      = @ModifiedBy
    WHERE Id = @Id;

    SELECT @Id;
END
