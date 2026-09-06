CREATE OR ALTER PROCEDURE dbo.GetLeagueById
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, Name, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy
    FROM dbo.Leagues
    WHERE Id = @Id;
END
