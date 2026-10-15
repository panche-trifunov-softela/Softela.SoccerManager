CREATE OR ALTER PROCEDURE dbo.UpdateManagerPlayerRatingVote
    @Id            INT,
    @UpOrDown      BIT,
    @ModifiedAt    DATETIME2(7),
    @ModifiedBy    UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    -- Only the direction changes: the manager, the player and the rating the vote was cast against stay fixed.
    UPDATE dbo.ManagerPlayerRatingVotes
    SET UpOrDown   = @UpOrDown,
        ModifiedAt = @ModifiedAt,
        ModifiedBy = @ModifiedBy
    WHERE Id = @Id;

    SELECT @Id;
END
