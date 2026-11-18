-- @Commentary is NVARCHAR(MAX), not JSON: it converts implicitly into the column's native json type on insert.
CREATE OR ALTER PROCEDURE dbo.InsertStagedMatch
    @SeasonId      INT,
    @CompetitionId INT,
    @RefereeId     INT,
    @StartDateTime DATETIME2(7),
    @Commentary    NVARCHAR(MAX),
    @Attendance    INT,
    @IsStarted     BIT,
    @Stage         TINYINT,
    @RoundNumber   TINYINT,
    @Leg           TINYINT,
    @GroupNumber   TINYINT = NULL,
    @CreatedAt     DATETIME2(7),
    @ModifiedAt    DATETIME2(7),
    @CreatedBy     UNIQUEIDENTIFIER,
    @ModifiedBy    UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    -- dbo.InsertMatch plus the match's place in a cup or playoff (V1_0_0_57). The Generator writes every cup and
    -- playoff match through it; league matches and friendlies keep using dbo.InsertMatch, so their Stage,
    -- RoundNumber and Leg stay NULL. The CK_Matches_* constraints judge these values. No C# in this repository
    -- calls it: the Generator does, so its parameters are a contract with the Generator. GroupNumber is the World Cup
    -- group (V1_0_0_69).
    -- A World Cup stage belongs to a national competition, and a national competition holds nothing else (V1_0_0_69).
    -- A World Cup stage with no competition at all is refused by CK_Matches_WorldCupStageHasCompetition.
    IF EXISTS (
        SELECT 1
        FROM dbo.Competitions c
        WHERE c.Id = @CompetitionId
          AND c.IsNational <> CASE WHEN @Stage BETWEEN 10 AND 16 THEN 1 ELSE 0 END)
        THROW 50000, 'A World Cup stage belongs to a national competition, and a national competition only has World Cup stages.', 1;

    INSERT INTO dbo.Matches (SeasonId, CompetitionId, RefereeId, StartDateTime, Commentary, Attendance, IsStarted, Stage, RoundNumber, Leg, GroupNumber, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy)
    OUTPUT INSERTED.Id
    VALUES (@SeasonId, @CompetitionId, @RefereeId, @StartDateTime, @Commentary, @Attendance, @IsStarted, @Stage, @RoundNumber, @Leg, @GroupNumber, @CreatedAt, @ModifiedAt, @CreatedBy, @ModifiedBy);
END
