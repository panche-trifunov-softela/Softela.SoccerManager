CREATE OR ALTER PROCEDURE dbo.GetMatchPlayerStatistics
    @MatchId INT
AS
BEGIN
    SET NOCOUNT ON;

    -- Ordered by Rating DESC so the match's best performer comes back first.
    SELECT Id, PlayerId, MatchId, Rating, IsStarter, MinutesPlayed, Goals, Assists, PenaltiesScored, PenaltiesMissed, YellowCards, RedCards, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy
    FROM dbo.MatchPlayerStatistics
    WHERE MatchId = @MatchId
    ORDER BY Rating DESC;
END
