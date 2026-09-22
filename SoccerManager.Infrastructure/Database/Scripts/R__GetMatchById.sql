CREATE OR ALTER PROCEDURE dbo.GetMatchById
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, SeasonId, CompetitionId, RefereeId, StartDateTime, Commentary, Attendance, IsStarted, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy
    FROM dbo.Matches
    WHERE Id = @Id;
END
