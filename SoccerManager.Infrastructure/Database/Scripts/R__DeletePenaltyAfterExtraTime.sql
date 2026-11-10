CREATE OR ALTER PROCEDURE dbo.DeletePenaltyAfterExtraTime
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    -- This is a hard delete; the table has no soft-delete flag.
    DELETE FROM dbo.PenaltiesAfterExtraTime WHERE Id = @Id;
END
