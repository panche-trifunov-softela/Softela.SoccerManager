-- A new rating becomes the player's current one. The previous current row is kept as history first, so
-- UX_PlayerRatings_PlayerId_Current never sees two; the votes stay on the rating they were cast on.
CREATE OR ALTER PROCEDURE dbo.InsertPlayerRating
    @PlayerId      INT,
    @Rating        INT,
    @CreatedAt     DATETIME2(7),
    @ModifiedAt    DATETIME2(7),
    @CreatedBy     UNIQUEIDENTIFIER,
    @ModifiedBy    UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.PlayerRatings
    SET IsCurrent  = 0,
        ModifiedAt = @ModifiedAt,
        ModifiedBy = @ModifiedBy
    WHERE PlayerId = @PlayerId
      AND IsCurrent = 1;

    INSERT INTO dbo.PlayerRatings (PlayerId, Rating, IsCurrent, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy)
    OUTPUT INSERTED.Id
    VALUES (@PlayerId, @Rating, 1, @CreatedAt, @ModifiedAt, @CreatedBy, @ModifiedBy);
END
