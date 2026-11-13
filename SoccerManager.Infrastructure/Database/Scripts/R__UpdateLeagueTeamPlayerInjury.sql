CREATE OR ALTER PROCEDURE dbo.UpdateLeagueTeamPlayerInjury
    @Id                 INT,
    @LeagueTeamPlayerId INT,
    @Name               NVARCHAR(100),
    @InjuryStartDate    DATETIME2(7),
    @InjuryEndDate      DATETIME2(7),
    @InjuryScale        TINYINT,
    @ModifiedAt         DATETIME2(7),
    @ModifiedBy         UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.LeagueTeamPlayerInjuries
    SET LeagueTeamPlayerId = @LeagueTeamPlayerId,
        Name               = @Name,
        InjuryStartDate    = @InjuryStartDate,
        InjuryEndDate      = @InjuryEndDate,
        InjuryScale        = @InjuryScale,
        ModifiedAt         = @ModifiedAt,
        ModifiedBy         = @ModifiedBy
    WHERE Id = @Id;

    SELECT @Id;
END
