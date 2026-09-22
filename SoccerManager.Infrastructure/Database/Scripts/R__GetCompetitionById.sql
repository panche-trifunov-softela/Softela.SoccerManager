CREATE OR ALTER PROCEDURE dbo.GetCompetitionById
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, LeagueId, Name, LogoUrl, IsDomestic, Format, MaxAgeAllowed, [Order], TeamsPromoted, TeamsRelegated, TeamsInPlayoffs, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy
    FROM dbo.Competitions
    WHERE Id = @Id;
END
