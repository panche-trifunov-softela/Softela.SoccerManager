CREATE OR ALTER PROCEDURE dbo.GetPenaltyAfterExtraTimeById
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, MatchId, KickNumber, LeagueTeamId, PlayerId, IsScored, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy
    FROM dbo.PenaltiesAfterExtraTime
    WHERE Id = @Id;
END
