CREATE OR ALTER PROCEDURE dbo.GetMatchTeamTactics
    @MatchId INT
AS
BEGIN
    SET NOCOUNT ON;

    -- Ordered by TeamId so the two rows of a match come back in a stable order.
    SELECT Id, MatchId, TeamId, Mentality, Tempo, Passing, Width, Pressing, Tackling, AttackingSide, PenaltyTakerPlayerId, FreeKickTakerPlayerId, CornerKickTakerPlayerId, CaptainPlayerId, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy
    FROM dbo.MatchTeamTactics
    WHERE MatchId = @MatchId
    ORDER BY TeamId;
END
