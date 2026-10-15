CREATE OR ALTER PROCEDURE dbo.GetManagerPlayerRatingVoteById
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, ManagerId, PlayerId, RatingAtTheMoment, UpOrDown, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy
    FROM dbo.ManagerPlayerRatingVotes
    WHERE Id = @Id;
END
