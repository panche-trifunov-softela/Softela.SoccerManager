CREATE OR ALTER PROCEDURE dbo.GetManagerPlayerRatingVoteByManagerAndPlayerRating
    @ManagerId         INT,
    @PlayerRatingId    INT
AS
BEGIN
    SET NOCOUNT ON;

    -- At most one row: UQ_ManagerPlayerRatingVotes_ManagerId_PlayerRatingId.
    SELECT Id, ManagerId, PlayerRatingId, UpOrDown, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy
    FROM dbo.ManagerPlayerRatingVotes
    WHERE ManagerId = @ManagerId
      AND PlayerRatingId = @PlayerRatingId;
END
