CREATE OR ALTER PROCEDURE dbo.GetLeagueTeamPlayerInjuries
    @LeagueTeamPlayerId INT
AS
BEGIN
    SET NOCOUNT ON;

    -- Ordered by InjuryStartDate DESC so the latest injury comes back first; Id DESC breaks a tie.
    SELECT Id, LeagueTeamPlayerId, Name, InjuryStartDate, InjuryEndDate, InjuryScale, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy
    FROM dbo.LeagueTeamPlayerInjuries
    WHERE LeagueTeamPlayerId = @LeagueTeamPlayerId
    ORDER BY InjuryStartDate DESC, Id DESC;
END
