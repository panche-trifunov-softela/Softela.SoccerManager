-- The rating is the player's current dbo.PlayerRatings row; every player has exactly one.
CREATE OR ALTER PROCEDURE dbo.GetPlayerById
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT p.Id, p.Name, p.DateOfBirth, cr.Rating, p.[Value], p.Wage, p.ImageUrl, p.NationalTeamId, p.TeamId, p.TransfermarktId, p.CreatedAt, p.ModifiedAt, p.CreatedBy, p.ModifiedBy
    FROM dbo.Players p
        INNER JOIN dbo.PlayerRatings cr ON cr.PlayerId = p.Id AND cr.IsCurrent = 1
    WHERE p.Id = @Id;
END
