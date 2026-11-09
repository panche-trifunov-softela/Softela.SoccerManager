-- Sets the four roles of one substitution's tactic row, after a lineup change took a role player off. It never touches the
-- instructions, the key columns or the creation columns.
CREATE OR ALTER PROCEDURE dbo.UpdateMatchTeamTacticSubstitutionRoles
    @Id                      INT,
    @PenaltyTakerPlayerId    INT,
    @FreeKickTakerPlayerId   INT,
    @CornerKickTakerPlayerId INT,
    @CaptainPlayerId         INT,
    @ModifiedAt              DATETIME2(7),
    @ModifiedBy              UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.MatchTeamTacticSubstitutions
    SET PenaltyTakerPlayerId    = @PenaltyTakerPlayerId,
        FreeKickTakerPlayerId   = @FreeKickTakerPlayerId,
        CornerKickTakerPlayerId = @CornerKickTakerPlayerId,
        CaptainPlayerId         = @CaptainPlayerId,
        ModifiedAt              = @ModifiedAt,
        ModifiedBy              = @ModifiedBy
    WHERE Id = @Id;
END
