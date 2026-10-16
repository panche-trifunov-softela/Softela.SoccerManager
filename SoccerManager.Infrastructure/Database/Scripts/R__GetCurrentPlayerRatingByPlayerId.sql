CREATE OR ALTER PROCEDURE dbo.GetCurrentPlayerRatingByPlayerId
    @PlayerId INT
AS
BEGIN
    SET NOCOUNT ON;

    -- At most one row: UX_PlayerRatings_PlayerId_Current.
    SELECT Id, PlayerId, Rating, IsCurrent, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy
    FROM dbo.PlayerRatings
    WHERE PlayerId = @PlayerId
      AND IsCurrent = 1;
END
