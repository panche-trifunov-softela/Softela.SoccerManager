CREATE OR ALTER PROCEDURE dbo.InsertManagerPlayerRatingVote
    @ManagerId         INT,
    @PlayerRatingId    INT,
    @UpOrDown          BIT,
    @CreatedAt         DATETIME2(7),
    @ModifiedAt        DATETIME2(7),
    @CreatedBy         UNIQUEIDENTIFIER,
    @ModifiedBy        UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.ManagerPlayerRatingVotes (ManagerId, PlayerRatingId, UpOrDown, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy)
    OUTPUT INSERTED.Id
    VALUES (@ManagerId, @PlayerRatingId, @UpOrDown, @CreatedAt, @ModifiedAt, @CreatedBy, @ModifiedBy);
END
