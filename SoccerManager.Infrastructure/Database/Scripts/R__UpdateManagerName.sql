-- Copies the caller's Keycloak first and last name onto their manager profile; POST /api/managers/me runs it right
-- after dbo.EnsureManager, inside the same transaction, so the row is still locked by EnsureManager's existence check.
-- It is a procedure of its own, and dbo.EnsureManager is left unchanged, because the database is shared with local
-- development: an older build re-applies its own text of any procedure it knows, which would strip new parameters
-- from EnsureManager under a running API, but it never touches a procedure it does not know.
-- A NULL @FirstName or @LastName means the token did not carry that name, and leaves the stored one alone. A supplied
-- name is written only when it differs from the stored one, compared with a binary collation so a change of case or of
-- accents alone, such as 'anna' to 'Anna', is still written; a visit with unchanged names writes nothing and leaves
-- ModifiedAt alone.
CREATE OR ALTER PROCEDURE dbo.UpdateManagerName
    @UserId     UNIQUEIDENTIFIER,
    @FirstName  NVARCHAR(255),
    @LastName   NVARCHAR(255),
    @ModifiedAt DATETIME2(7),
    @ModifiedBy UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.Managers
    SET FirstName  = COALESCE(@FirstName, FirstName),
        LastName   = COALESCE(@LastName, LastName),
        ModifiedAt = @ModifiedAt,
        ModifiedBy = @ModifiedBy
    WHERE UserId = @UserId
      AND (   (@FirstName IS NOT NULL AND (FirstName IS NULL OR FirstName COLLATE Latin1_General_100_BIN2 <> @FirstName))
           OR (@LastName  IS NOT NULL AND (LastName  IS NULL OR LastName  COLLATE Latin1_General_100_BIN2 <> @LastName)));
END
