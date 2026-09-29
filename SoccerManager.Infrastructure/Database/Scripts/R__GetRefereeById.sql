CREATE OR ALTER PROCEDURE dbo.GetRefereeById
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, Name, ImageUrl, Tolerance, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy
    FROM dbo.Referees
    WHERE Id = @Id;
END
