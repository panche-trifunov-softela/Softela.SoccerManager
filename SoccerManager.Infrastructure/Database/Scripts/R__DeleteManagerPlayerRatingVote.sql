CREATE OR ALTER PROCEDURE dbo.DeleteManagerPlayerRatingVote
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM dbo.ManagerPlayerRatingVotes WHERE Id = @Id;
END
