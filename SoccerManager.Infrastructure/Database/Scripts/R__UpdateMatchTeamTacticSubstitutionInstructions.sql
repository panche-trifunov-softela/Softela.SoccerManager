-- Changes only the sent instructions of one substitution's tactic row: a NULL parameter keeps its column. It never touches the
-- four roles, the key columns or the creation columns.
CREATE OR ALTER PROCEDURE dbo.UpdateMatchTeamTacticSubstitutionInstructions
    @Id            INT,
    @Mentality     TINYINT,
    @Tempo         TINYINT,
    @Passing       TINYINT,
    @Width         TINYINT,
    @Pressing      TINYINT,
    @Tackling      TINYINT,
    @AttackingSide TINYINT,
    @ModifiedAt    DATETIME2(7),
    @ModifiedBy    UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.MatchTeamTacticSubstitutions
    SET Mentality     = COALESCE(@Mentality, Mentality),
        Tempo         = COALESCE(@Tempo, Tempo),
        Passing       = COALESCE(@Passing, Passing),
        Width         = COALESCE(@Width, Width),
        Pressing      = COALESCE(@Pressing, Pressing),
        Tackling      = COALESCE(@Tackling, Tackling),
        AttackingSide = COALESCE(@AttackingSide, AttackingSide),
        ModifiedAt    = @ModifiedAt,
        ModifiedBy    = @ModifiedBy
    WHERE Id = @Id;
END
