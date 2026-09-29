CREATE OR ALTER PROCEDURE dbo.GetNationalTeamById
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, Name, StadiumId, JerseyUrl, LogoUrl, TransfermarktId, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy
    FROM dbo.NationalTeams
    WHERE Id = @Id;
END
