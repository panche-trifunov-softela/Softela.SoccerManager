CREATE OR ALTER PROCEDURE dbo.GetMatchPlayerStatisticById
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, PlayerId, MatchId, LeagueTeamId, Rating, IsStarter, MinutesPlayed, Goals, OwnGoals, Assists, PenaltiesScored, PenaltiesMissed, YellowCards, RedCards, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy
    FROM dbo.MatchPlayerStatistics
    WHERE Id = @Id;
END
