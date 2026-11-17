-- Every national team with its confederation, NULL when none is set.
-- A team without a confederation is returned too, not left out, so the Generator can report each team it cannot place.
-- This never writes. No C# in this repository calls it: the Generator does, so its columns are a contract with the Generator.
CREATE OR ALTER PROCEDURE dbo.GetNationalTeamConfederations
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id,
           Name,
           Confederation
    FROM dbo.NationalTeams
    ORDER BY Id;
END
