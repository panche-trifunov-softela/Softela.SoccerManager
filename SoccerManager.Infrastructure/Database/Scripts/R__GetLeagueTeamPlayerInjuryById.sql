CREATE OR ALTER PROCEDURE dbo.GetLeagueTeamPlayerInjuryById
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, LeagueTeamPlayerId, Name, InjuryStartDate, InjuryEndDate, InjuryScale, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy
    FROM dbo.LeagueTeamPlayerInjuries
    WHERE Id = @Id;
END
