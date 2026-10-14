CREATE OR ALTER PROCEDURE dbo.UpdateMatchFormationPlayerPosition
    @Id                       INT,
    @MatchId                  INT,
    @LeagueTeamId             INT,
    @FormationPositionId      INT,
    @PlayerPositionId         INT,
    @ConditionOnMatch         INT,
    @QualityAtPositionOnMatch INT,
    @IsSuspended              BIT,
    @ModifiedAt               DATETIME2(7),
    @ModifiedBy               UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    -- The same guard as dbo.InsertMatchFormationPlayerPosition: an update can move the row to another
    -- match or club.
    IF NOT EXISTS (
        SELECT 1
        FROM dbo.LeagueTeams lt
            INNER JOIN dbo.Seasons se ON se.LeagueId = lt.LeagueId
            INNER JOIN dbo.Matches m ON m.SeasonId = se.Id
        WHERE lt.Id = @LeagueTeamId AND m.Id = @MatchId)
        THROW 50000, 'The club and the match must belong to the same league.', 1;

    UPDATE dbo.MatchFormationPlayerPositions
    SET MatchId                  = @MatchId,
        LeagueTeamId             = @LeagueTeamId,
        FormationPositionId      = @FormationPositionId,
        PlayerPositionId         = @PlayerPositionId,
        ConditionOnMatch         = @ConditionOnMatch,
        QualityAtPositionOnMatch = @QualityAtPositionOnMatch,
        IsSuspended              = @IsSuspended,
        ModifiedAt               = @ModifiedAt,
        ModifiedBy               = @ModifiedBy
    WHERE Id = @Id;

    SELECT @Id;
END
