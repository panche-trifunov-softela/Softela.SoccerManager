CREATE OR ALTER PROCEDURE dbo.GetSeasons
    @LeagueId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, LeagueId, SeasonNumber, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy
    FROM dbo.Seasons
    WHERE LeagueId = @LeagueId
    ORDER BY SeasonNumber;
END
