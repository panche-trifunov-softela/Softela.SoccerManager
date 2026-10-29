-- @OwnerManagerId is last and defaults to NULL so an older API build on the shared database,
-- which does not pass it, can still create a league; that league has no owner.
CREATE OR ALTER PROCEDURE dbo.InsertLeague
    @Name           NVARCHAR(100),
    @CreatedAt      DATETIME2(7),
    @ModifiedAt     DATETIME2(7),
    @CreatedBy      UNIQUEIDENTIFIER,
    @ModifiedBy     UNIQUEIDENTIFIER,
    @OwnerManagerId INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.Leagues (Name, OwnerManagerId, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy)
    OUTPUT INSERTED.Id
    VALUES (@Name, @OwnerManagerId, @CreatedAt, @ModifiedAt, @CreatedBy, @ModifiedBy);
END
