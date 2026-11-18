CREATE OR ALTER PROCEDURE dbo.GetNationalTeamMatches
    @SeasonId INT
AS
BEGIN
    SET NOCOUNT ON;

    -- In kickoff order, so a season's World Cup reads from its first match to its last; Id orders matches that kick off together. This never writes.
    SELECT Id, SeasonId, HomeNationalTeamId, AwayNationalTeamId, StartDateTime, Stage, GroupNumber, RoundNumber, Leg, IsStarted, IsFinished, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy
    FROM dbo.NationalTeamMatches
    WHERE SeasonId = @SeasonId
    ORDER BY StartDateTime, Id;
END
