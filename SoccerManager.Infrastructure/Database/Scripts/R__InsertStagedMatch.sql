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
    @CreatedAt     DATETIME2(7),
    @ModifiedAt    DATETIME2(7),
    @CreatedBy     UNIQUEIDENTIFIER,
    @ModifiedBy    UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    -- dbo.InsertMatch plus the match's place in a cup or playoff (V1_0_0_57). The Generator writes every cup and
    -- playoff match through it; league matches and friendlies keep using dbo.InsertMatch, so their Stage,
    -- RoundNumber and Leg stay NULL. The CK_Matches_* constraints judge the three values. No C# in this repository
    -- calls it: the Generator does, so its parameters are a contract with the Generator.
    INSERT INTO dbo.Matches (SeasonId, CompetitionId, RefereeId, StartDateTime, Commentary, Attendance, IsStarted, Stage, RoundNumber, Leg, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy)
    OUTPUT INSERTED.Id
    VALUES (@SeasonId, @CompetitionId, @RefereeId, @StartDateTime, @Commentary, @Attendance, @IsStarted, @Stage, @RoundNumber, @Leg, @CreatedAt, @ModifiedAt, @CreatedBy, @ModifiedBy);
END
