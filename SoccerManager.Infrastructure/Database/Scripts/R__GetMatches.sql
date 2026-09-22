CREATE OR ALTER PROCEDURE dbo.GetMatches
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, SeasonId, CompetitionId, RefereeId, StartDateTime, Commentary, Attendance, IsStarted, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy
    FROM dbo.Matches
    ORDER BY StartDateTime DESC;
END
