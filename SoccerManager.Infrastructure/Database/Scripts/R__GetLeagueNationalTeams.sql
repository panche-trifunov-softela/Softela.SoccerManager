-- Every national team row on one League's roster (V1_0_0_68): the row's Id and its NationalTeamId.
-- dbo.GetLeagueTeams returns the clubs only, so the Generator learns from this read which roster row is each nation in its World Cup matches.
-- This never writes. No C# in this repository calls it: the Generator does, so its columns are a contract with the Generator.
CREATE OR ALTER PROCEDURE dbo.GetLeagueNationalTeams
    @LeagueId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id,
           NationalTeamId
    FROM dbo.LeagueTeams
    WHERE LeagueId = @LeagueId
      AND NationalTeamId IS NOT NULL
    ORDER BY Id;
END
