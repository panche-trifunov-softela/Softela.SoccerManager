CREATE OR ALTER PROCEDURE dbo.GetNationalTeamMatchById
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, SeasonId, HomeNationalTeamId, AwayNationalTeamId, StartDateTime, Stage, GroupNumber, RoundNumber, Leg, IsStarted, IsFinished, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy
    FROM dbo.NationalTeamMatches
    WHERE Id = @Id;
END
