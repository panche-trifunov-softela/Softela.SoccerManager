CREATE OR ALTER PROCEDURE dbo.GetMatchPlayerStatisticById
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, PlayerId, MatchId, Rating, IsStarter, MinutesPlayed, Goals, Assists, PenaltiesScored, PenaltiesMissed, YellowCards, RedCards, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy
    FROM dbo.MatchPlayerStatistics
    WHERE Id = @Id;
END
