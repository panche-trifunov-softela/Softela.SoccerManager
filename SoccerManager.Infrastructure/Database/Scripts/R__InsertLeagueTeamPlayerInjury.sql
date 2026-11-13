CREATE OR ALTER PROCEDURE dbo.InsertLeagueTeamPlayerInjury
    @LeagueTeamPlayerId INT,
    @Name               NVARCHAR(100),
    @InjuryStartDate    DATETIME2(7),
    @InjuryEndDate      DATETIME2(7),
    @InjuryScale        TINYINT,
    @CreatedAt          DATETIME2(7),
    @ModifiedAt         DATETIME2(7),
    @CreatedBy          UNIQUEIDENTIFIER,
    @ModifiedBy         UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.LeagueTeamPlayerInjuries (LeagueTeamPlayerId, Name, InjuryStartDate, InjuryEndDate, InjuryScale, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy)
    OUTPUT INSERTED.Id
    VALUES (@LeagueTeamPlayerId, @Name, @InjuryStartDate, @InjuryEndDate, @InjuryScale, @CreatedAt, @ModifiedAt, @CreatedBy, @ModifiedBy);
END
