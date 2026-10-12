CREATE OR ALTER PROCEDURE dbo.InsertLeagueTeam
    @LeagueId   INT,
    @TeamId     INT,
    @CreatedAt  DATETIME2(7),
    @ModifiedAt DATETIME2(7),
    @CreatedBy  UNIQUEIDENTIFIER,
    @ModifiedBy UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.LeagueTeams (LeagueId, TeamId, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy)
    OUTPUT INSERTED.Id
    VALUES (@LeagueId, @TeamId, @CreatedAt, @ModifiedAt, @CreatedBy, @ModifiedBy);
END
