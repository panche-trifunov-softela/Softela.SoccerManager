CREATE OR ALTER PROCEDURE dbo.GetPlayers
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, Name, DateOfBirth, Rating, [Value], Wage, ImageUrl, NationalTeamId, TeamId, TransfermarktId, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy
    FROM dbo.Players
    ORDER BY Name;
END
