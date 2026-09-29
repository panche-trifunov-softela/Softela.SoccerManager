CREATE OR ALTER PROCEDURE dbo.GetTeamById
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, Name, StadiumId, FinancialState, JerseyUrl, LogoUrl, TransfermarktId, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy
    FROM dbo.Teams
    WHERE Id = @Id;
END
