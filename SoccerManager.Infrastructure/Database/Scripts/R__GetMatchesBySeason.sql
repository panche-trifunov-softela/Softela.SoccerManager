CREATE OR ALTER PROCEDURE dbo.GetMatchesBySeason
    @SeasonId INT
AS
BEGIN
    SET NOCOUNT ON;

    -- Every match of the season, friendlies included, for the Generator's cup, playoff and rollover steps. It has
    -- dbo.GetMatches' columns without Commentary, which no step reads and which is the one large column, plus
    -- Stage, RoundNumber and Leg (NULL on league matches and friendlies). This never writes. No C# in this
    -- repository calls it: the Generator does, so its columns are a contract with the Generator.
    SELECT Id, SeasonId, CompetitionId, RefereeId, StartDateTime, Attendance, IsStarted, IsFinished, Stage, RoundNumber, Leg, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy
    FROM dbo.Matches
    WHERE SeasonId = @SeasonId
    ORDER BY StartDateTime, Id;
END
