-- Changes only the sent instructions of one club's tactic row for one match, behind PUT /api/matches/{id}/instructions: a NULL
-- parameter keeps its column. It is one statement, so two saves of different instructions both persist, unlike
-- dbo.UpdateMatchTeamTactic, which overwrites the whole row. It never touches the four roles, the match, the club or the
-- creation columns. It returns the row's seven instructions after the change, in the order of
-- dbo.GetMatchTeamTacticInstructions; no row for the club and match = nothing updated and nothing returned.
CREATE OR ALTER PROCEDURE dbo.UpdateMatchTeamTacticInstructions
    @MatchId       INT,
    @LeagueTeamId  INT,
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

    UPDATE dbo.MatchTeamTactics
    SET Mentality     = COALESCE(@Mentality, Mentality),
        Tempo         = COALESCE(@Tempo, Tempo),
        Passing       = COALESCE(@Passing, Passing),
        Width         = COALESCE(@Width, Width),
        Pressing      = COALESCE(@Pressing, Pressing),
        Tackling      = COALESCE(@Tackling, Tackling),
        AttackingSide = COALESCE(@AttackingSide, AttackingSide),
        ModifiedAt    = @ModifiedAt,
        ModifiedBy    = @ModifiedBy
    OUTPUT INSERTED.Mentality,
           INSERTED.Tempo,
           INSERTED.Passing,
           INSERTED.Width,
           INSERTED.Pressing,
           INSERTED.Tackling,
           INSERTED.AttackingSide
    WHERE MatchId = @MatchId AND LeagueTeamId = @LeagueTeamId;
END
