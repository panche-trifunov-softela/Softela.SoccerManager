CREATE OR ALTER PROCEDURE dbo.GetManagerPlayerRatingVotes
    @PlayerId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, ManagerId, PlayerId, RatingAtTheMoment, UpOrDown, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy
    FROM dbo.ManagerPlayerRatingVotes
    WHERE PlayerId = @PlayerId
    ORDER BY CreatedAt DESC, Id DESC;
END
