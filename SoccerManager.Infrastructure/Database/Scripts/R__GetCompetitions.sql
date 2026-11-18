-- IsNational (V1_0_0_68) marks a national competition, the World Cup; the Generator reads it to keep such a competition out of its club cup shapes.
CREATE OR ALTER PROCEDURE dbo.GetCompetitions
    @LeagueId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, LeagueId, Name, LogoUrl, IsDomestic, Format, IsNational, MaxAgeAllowed, [Order], TeamsPromoted, TeamsRelegated, TeamsInPlayoffs, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy
    FROM dbo.Competitions
    WHERE LeagueId = @LeagueId
    ORDER BY Name;
END
