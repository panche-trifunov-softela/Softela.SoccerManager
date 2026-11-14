CREATE OR ALTER PROCEDURE dbo.GetMatchPlayerAbsences
    @MatchId INT
AS
BEGIN
    SET NOCOUNT ON;

    -- Ordered by club, then player, so each club's absences come back together.
    SELECT Id, MatchId, LeagueTeamId, PlayerId, Reason, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy
    FROM dbo.MatchPlayerAbsences
    WHERE MatchId = @MatchId
    ORDER BY LeagueTeamId, PlayerId;
END
