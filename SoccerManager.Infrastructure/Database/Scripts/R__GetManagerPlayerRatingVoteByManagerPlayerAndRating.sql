CREATE OR ALTER PROCEDURE dbo.GetManagerPlayerRatingVoteByManagerPlayerAndRating
    @ManagerId            INT,
    @PlayerId             INT,
    @RatingAtTheMoment    INT
AS
BEGIN
    SET NOCOUNT ON;

    -- At most one row: UQ_ManagerPlayerRatingVotes_ManagerId_PlayerId_RatingAtTheMoment.
    SELECT Id, ManagerId, PlayerId, RatingAtTheMoment, UpOrDown, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy
    FROM dbo.ManagerPlayerRatingVotes
    WHERE ManagerId = @ManagerId
      AND PlayerId = @PlayerId
      AND RatingAtTheMoment = @RatingAtTheMoment;
END
