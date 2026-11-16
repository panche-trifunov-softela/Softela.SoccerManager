-- Returns a league's latest season, the one a new bid is placed in. The latest is the one with the highest SeasonNumber: a league's
-- seasons are numbered in order and UQ_Seasons_LeagueId_SeasonNumber keeps each number once, and a season has no date or
-- current flag to say otherwise. A league with no season gives no row.
CREATE OR ALTER PROCEDURE dbo.GetLatestSeason
    @LeagueId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP (1) Id, LeagueId, SeasonNumber, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy
    FROM dbo.Seasons
    WHERE LeagueId = @LeagueId
    ORDER BY SeasonNumber DESC;
END
