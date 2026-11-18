-- @Commentary is NVARCHAR(MAX), not JSON: it converts implicitly into the column's native json type on update.
CREATE OR ALTER PROCEDURE dbo.UpdateMatch
    @Id            INT,
    @SeasonId      INT,
    @CompetitionId INT,
    @RefereeId     INT,
    @StartDateTime DATETIME2(7),
    @Commentary    NVARCHAR(MAX),
    @Attendance    INT,
    @IsStarted     BIT,
    @IsFinished    BIT,
    @ModifiedAt    DATETIME2(7),
    @ModifiedBy    UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    -- A match cannot move into or out of a national competition: its stage and its sides were written for the kind of
    -- competition it is in (V1_0_0_69). A friendly between two national teams cannot move into a club competition either.
    IF EXISTS (
        SELECT 1
        FROM dbo.Matches m
            LEFT JOIN dbo.Competitions oc ON oc.Id = m.CompetitionId
            LEFT JOIN dbo.Competitions nc ON nc.Id = @CompetitionId
        WHERE m.Id = @Id
          AND (ISNULL(oc.IsNational, 0) <> ISNULL(nc.IsNational, 0)
               OR (nc.Id IS NOT NULL AND EXISTS (
                   SELECT 1
                   FROM dbo.MatchTeamStatistics s
                       INNER JOIN dbo.LeagueTeams slt ON slt.Id = s.LeagueTeamId
                   WHERE s.MatchId = m.Id
                     AND CASE WHEN slt.NationalTeamId IS NULL THEN 0 ELSE 1 END <> nc.IsNational))))
        THROW 50000, 'A match cannot move into or out of a national competition, and national teams cannot play in a club competition.', 1;

    UPDATE dbo.Matches
    SET SeasonId      = @SeasonId,
        CompetitionId = @CompetitionId,
        RefereeId     = @RefereeId,
        StartDateTime = @StartDateTime,
        Commentary    = @Commentary,
        Attendance    = @Attendance,
        IsStarted     = @IsStarted,
        IsFinished    = @IsFinished,
        ModifiedAt    = @ModifiedAt,
        ModifiedBy    = @ModifiedBy
    WHERE Id = @Id;

    SELECT @Id;
END
