CREATE OR ALTER PROCEDURE dbo.UpdateDivision
    @Id              INT,
    @Name            NVARCHAR(100),
    @Order           INT,
    @TeamsPromoted   INT,
    @TeamsRelegated  INT,
    @TeamsInPlayoffs INT,
    @ModifiedAt      DATETIME2(7),
    @ModifiedBy      UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    -- LeagueId is fixed at creation, so a division never moves between leagues.
    UPDATE dbo.Divisions
    SET Name            = @Name,
        [Order]         = @Order,
        TeamsPromoted   = @TeamsPromoted,
        TeamsRelegated  = @TeamsRelegated,
        TeamsInPlayoffs = @TeamsInPlayoffs,
        ModifiedAt      = @ModifiedAt,
        ModifiedBy      = @ModifiedBy
    WHERE Id = @Id;

    SELECT @Id;
END
