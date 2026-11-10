CREATE OR ALTER PROCEDURE dbo.GetPenaltiesAfterExtraTime
    @MatchId INT
AS
BEGIN
    SET NOCOUNT ON;

    -- In kick order, so the shootout reads from the first kick to the last. This never writes.
    SELECT Id, MatchId, KickNumber, LeagueTeamId, PlayerId, IsScored, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy
    FROM dbo.PenaltiesAfterExtraTime
    WHERE MatchId = @MatchId
    ORDER BY KickNumber;
END
