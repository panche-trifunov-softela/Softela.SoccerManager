CREATE OR ALTER PROCEDURE dbo.GetCompetitions
    @LeagueId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, LeagueId, Name, LogoUrl, IsDomestic, Format, MaxAgeAllowed, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy
    FROM dbo.Competitions
    WHERE LeagueId = @LeagueId
    ORDER BY Name;
END
