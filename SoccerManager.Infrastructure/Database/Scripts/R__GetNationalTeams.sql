CREATE OR ALTER PROCEDURE dbo.GetNationalTeams
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, Name, StadiumId, JerseyUrl, LogoUrl, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy
    FROM dbo.NationalTeams
    ORDER BY Name;
END
