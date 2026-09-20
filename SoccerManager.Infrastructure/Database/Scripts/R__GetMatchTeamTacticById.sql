CREATE OR ALTER PROCEDURE dbo.GetMatchTeamTacticById
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, MatchId, TeamId, Mentality, Tempo, Passing, Width, Pressing, Tackling, AttackingSide, PenaltyTakerPlayerId, FreeKickTakerPlayerId, CornerKickTakerPlayerId, CaptainPlayerId, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy
    FROM dbo.MatchTeamTactics
    WHERE Id = @Id;
END
