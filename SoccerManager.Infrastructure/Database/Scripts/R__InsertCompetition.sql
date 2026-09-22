CREATE OR ALTER PROCEDURE dbo.InsertCompetition
    @LeagueId        INT,
    @Name            NVARCHAR(100),
    @LogoUrl         NVARCHAR(500),
    @IsDomestic      BIT,
    @Format          TINYINT,
    @MaxAgeAllowed   INT,
    @Order           INT,
    @TeamsPromoted   INT,
    @TeamsRelegated  INT,
    @TeamsInPlayoffs INT,
    @CreatedAt       DATETIME2(7),
    @ModifiedAt      DATETIME2(7),
    @CreatedBy       UNIQUEIDENTIFIER,
    @ModifiedBy      UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.Competitions (LeagueId, Name, LogoUrl, IsDomestic, Format, MaxAgeAllowed, [Order], TeamsPromoted, TeamsRelegated, TeamsInPlayoffs, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy)
    OUTPUT INSERTED.Id
    VALUES (@LeagueId, @Name, @LogoUrl, @IsDomestic, @Format, @MaxAgeAllowed, @Order, @TeamsPromoted, @TeamsRelegated, @TeamsInPlayoffs, @CreatedAt, @ModifiedAt, @CreatedBy, @ModifiedBy);
END
