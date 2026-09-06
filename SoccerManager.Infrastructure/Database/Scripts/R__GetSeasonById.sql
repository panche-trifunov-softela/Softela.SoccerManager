CREATE OR ALTER PROCEDURE dbo.GetSeasonById
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, LeagueId, SeasonNumber, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy
    FROM dbo.Seasons
    WHERE Id = @Id;
END
