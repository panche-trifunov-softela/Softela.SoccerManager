-- Every vote on any of the player's ratings: a vote reaches its player through dbo.PlayerRatings.
CREATE OR ALTER PROCEDURE dbo.GetManagerPlayerRatingVotes
    @PlayerId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT v.Id, v.ManagerId, v.PlayerRatingId, v.UpOrDown, v.CreatedAt, v.ModifiedAt, v.CreatedBy, v.ModifiedBy
    FROM dbo.ManagerPlayerRatingVotes v
        INNER JOIN dbo.PlayerRatings pr ON pr.Id = v.PlayerRatingId
    WHERE pr.PlayerId = @PlayerId
    ORDER BY v.CreatedAt DESC, v.Id DESC;
END
