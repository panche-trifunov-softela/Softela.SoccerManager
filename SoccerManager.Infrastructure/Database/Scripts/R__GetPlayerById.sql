CREATE OR ALTER PROCEDURE dbo.GetPlayerById
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, Name, DateOfBirth, Rating, [Value], Wage, ImageUrl, NationalTeamId, TeamId, TransfermarktId, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy
    FROM dbo.Players
    WHERE Id = @Id;
END
