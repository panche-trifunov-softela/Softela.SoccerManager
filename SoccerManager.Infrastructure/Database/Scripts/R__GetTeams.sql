CREATE OR ALTER PROCEDURE dbo.GetTeams
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, Name, StadiumId, FinancialState, JerseyUrl, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy
    FROM dbo.Teams
    ORDER BY Name;
END
