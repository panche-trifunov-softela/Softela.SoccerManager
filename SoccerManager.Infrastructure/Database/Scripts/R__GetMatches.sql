CREATE OR ALTER PROCEDURE dbo.GetMatches
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, SeasonId, DivisionId, RefereeId, StartDateTime, Commentary, Attendance, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy
    FROM dbo.Matches
    ORDER BY StartDateTime DESC;
END
