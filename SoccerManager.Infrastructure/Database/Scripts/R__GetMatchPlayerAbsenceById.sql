CREATE OR ALTER PROCEDURE dbo.GetMatchPlayerAbsenceById
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, MatchId, LeagueTeamId, PlayerId, Reason, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy
    FROM dbo.MatchPlayerAbsences
    WHERE Id = @Id;
END
