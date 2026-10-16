CREATE OR ALTER PROCEDURE dbo.UpdatePlayerRating
    @Id            INT,
    @Rating        INT,
    @ModifiedAt    DATETIME2(7),
    @ModifiedBy    UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    -- A correction of the value only: whether the rating is current, and the votes cast on it, stay as they are.
    UPDATE dbo.PlayerRatings
    SET Rating     = @Rating,
        ModifiedAt = @ModifiedAt,
        ModifiedBy = @ModifiedBy
    WHERE Id = @Id;

    SELECT @Id;
END
