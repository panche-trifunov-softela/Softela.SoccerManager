CREATE OR ALTER PROCEDURE dbo.GetMatchTeamStatisticById
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, TeamId, MatchId, IsHomeTeam, ShotsTotal, ShotsOnTarget, Possession, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy
    FROM dbo.MatchTeamStatistics
    WHERE Id = @Id;
END
