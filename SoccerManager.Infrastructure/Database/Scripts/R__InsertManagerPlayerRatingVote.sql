CREATE OR ALTER PROCEDURE dbo.InsertManagerPlayerRatingVote
    @ManagerId            INT,
    @PlayerId             INT,
    @RatingAtTheMoment    INT,
    @UpOrDown             BIT,
    @CreatedAt            DATETIME2(7),
    @ModifiedAt           DATETIME2(7),
    @CreatedBy            UNIQUEIDENTIFIER,
    @ModifiedBy           UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.ManagerPlayerRatingVotes (ManagerId, PlayerId, RatingAtTheMoment, UpOrDown, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy)
    OUTPUT INSERTED.Id
    VALUES (@ManagerId, @PlayerId, @RatingAtTheMoment, @UpOrDown, @CreatedAt, @ModifiedAt, @CreatedBy, @ModifiedBy);
END
