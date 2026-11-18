-- Gives each club of a League that has no squad rows yet its default squad: the real players of its Team who are on no squad of that League.
-- Guests always get one; division clubs only when @IncludeDivisionClubs = 1. A club keeps its best 99 by current rating, and each player gets a
-- squad number from the preferences of their main position, then the first free one. Contract length is 1 to 6 from a hash of the League and
-- player; Morale is Satisfied from rating 76 and VerySatisfied above 90, otherwise Normal; salary and value are the player's Wage and Value rounded.
-- V1_0_0_65 holds a copy of the squad block for its backfill, so a change here must be made there too (in a new migration, since V1_0_0_65 is immutable).
CREATE OR ALTER PROCEDURE dbo.InsertDefaultLeagueTeamPlayers
    @LeagueId             INT,
    @IncludeDivisionClubs BIT,
    @CreatedAt            DATETIME2(7),
    @CreatedBy            UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    CREATE TABLE #Clubs (LeagueTeamId INT NOT NULL PRIMARY KEY, LeagueId INT NOT NULL, TeamId INT NOT NULL);

    INSERT INTO #Clubs (LeagueTeamId, LeagueId, TeamId)
    SELECT lt.Id, lt.LeagueId, lt.TeamId
    FROM dbo.LeagueTeams lt
    WHERE lt.LeagueId = @LeagueId
      -- National team rows (V1_0_0_68) have no squad and no TeamId.
      AND lt.TeamId IS NOT NULL
      AND (lt.IsGuest = 1 OR @IncludeDivisionClubs = 1)
      AND NOT EXISTS (SELECT 1 FROM dbo.LeagueTeamPlayers ltp WHERE ltp.LeagueTeamId = lt.Id);

    -- SQUAD BLOCK: identical to the one in V1_0_0_65__add_league_team_guests.sql; only the input set and the audit values differ.
    -- Players already on a squad of the League: the one-club-per-League rule has no database key since V1_0_0_37.
    CREATE TABLE #Taken (LeagueId INT NOT NULL, PlayerId INT NOT NULL, PRIMARY KEY (LeagueId, PlayerId));

    INSERT INTO #Taken (LeagueId, PlayerId)
    SELECT DISTINCT lt.LeagueId, ltp.PlayerId
    FROM dbo.LeagueTeamPlayers ltp
        INNER JOIN dbo.LeagueTeams lt ON lt.Id = ltp.LeagueTeamId
    WHERE lt.LeagueId IN (SELECT LeagueId FROM #Clubs);

    -- The candidates, best rating first; a club keeps its best 99, the most squad numbers there are.
    CREATE TABLE #Squad (
        LeagueTeamId INT          NOT NULL,
        LeagueId     INT          NOT NULL,
        PlayerId     INT          NOT NULL,
        Rating       INT          NOT NULL,
        ShortName    NVARCHAR(10)     NULL,
        Rn           INT          NOT NULL,
        SquadNumber  INT              NULL,
        PRIMARY KEY (LeagueTeamId, Rn)
    );

    INSERT INTO #Squad (LeagueTeamId, LeagueId, PlayerId, Rating, ShortName, Rn)
    SELECT c.LeagueTeamId,
           c.LeagueId,
           p.Id,
           ISNULL(r.Rating, 0),
           mp.ShortName,
           ROW_NUMBER() OVER (PARTITION BY c.LeagueTeamId ORDER BY ISNULL(r.Rating, 0) DESC, p.Id)
    FROM #Clubs c
        INNER JOIN dbo.Players p ON p.TeamId = c.TeamId
        LEFT JOIN dbo.PlayerRatings r ON r.PlayerId = p.Id AND r.IsCurrent = 1
        OUTER APPLY (
            SELECT TOP (1) pos.ShortName
            FROM dbo.PlayerPositions pp
                INNER JOIN dbo.Positions pos ON pos.Id = pp.PositionId
            WHERE pp.PlayerId = p.Id
            ORDER BY pp.Quality DESC, pp.Id
        ) mp
    WHERE NOT EXISTS (SELECT 1 FROM #Taken tk WHERE tk.LeagueId = c.LeagueId AND tk.PlayerId = p.Id);

    DELETE FROM #Squad WHERE Rn > 99;

    -- Preferred squad numbers by main position, in order of preference.
    CREATE TABLE #Preferred (
        ShortName NVARCHAR(10) NOT NULL,
        Ord       INT          NOT NULL,
        Number    INT          NOT NULL,
        PRIMARY KEY (ShortName, Ord)
    );

    INSERT INTO #Preferred (ShortName, Ord, Number)
    VALUES ('GK', 1, 1), ('GK', 2, 13), ('GK', 3, 12),
           ('RB', 1, 2),
           ('LB', 1, 3),
           ('CB', 1, 4), ('CB', 2, 5), ('CB', 3, 6),
           ('DM', 1, 6), ('DM', 2, 5),
           ('CM', 1, 8), ('CM', 2, 6),
           ('AM', 1, 10), ('AM', 2, 8),
           ('LW', 1, 7), ('LW', 2, 11), ('LW', 3, 17),
           ('RW', 1, 7), ('RW', 2, 11), ('RW', 3, 17),
           ('CF', 1, 9), ('CF', 2, 19),
           ('SS', 1, 10), ('SS', 2, 9),
           ('LM', 1, 11), ('LM', 2, 7), ('LM', 3, 17),
           ('RM', 1, 7), ('RM', 2, 11), ('RM', 3, 17);

    -- Everyone's fallback after the preferred list: 14 to 40, then 41 to 99, then 1 to 13.
    CREATE TABLE #Fallback (Number INT NOT NULL PRIMARY KEY, Ord INT NOT NULL);

    WITH Numbers AS (
        SELECT 1 AS Number
        UNION ALL
        SELECT Number + 1 FROM Numbers WHERE Number < 99
    )
    INSERT INTO #Fallback (Number, Ord)
    SELECT Number, CASE WHEN Number >= 14 THEN Number - 13 ELSE Number + 86 END
    FROM Numbers;

    -- One rank at a time: a club has at most one player per rank, so a pass never clashes inside a club.
    CREATE TABLE #Used (LeagueTeamId INT NOT NULL, Number INT NOT NULL, PRIMARY KEY (LeagueTeamId, Number));

    DECLARE @Rank INT = 1, @MaxRank INT = (SELECT ISNULL(MAX(Rn), 0) FROM #Squad);

    WHILE @Rank <= @MaxRank
    BEGIN
        UPDATE s
        SET s.SquadNumber = pick.Number
        FROM #Squad s
            CROSS APPLY (
                SELECT TOP (1) x.Number
                FROM (
                    SELECT pr.Number, 0 AS Tier, pr.Ord FROM #Preferred pr WHERE pr.ShortName = s.ShortName
                    UNION ALL
                    SELECT f.Number, 1, f.Ord FROM #Fallback f
                ) x
                WHERE NOT EXISTS (SELECT 1 FROM #Used u WHERE u.LeagueTeamId = s.LeagueTeamId AND u.Number = x.Number)
                ORDER BY x.Tier, x.Ord
            ) pick
        WHERE s.Rn = @Rank;

        INSERT INTO #Used (LeagueTeamId, Number)
        SELECT LeagueTeamId, SquadNumber FROM #Squad WHERE Rn = @Rank;

        SET @Rank += 1;
    END;

    -- The contract length comes from a hash of the League and player, so it is 1 to 6 and the same every time.
    INSERT INTO dbo.LeagueTeamPlayers (LeagueTeamId, PlayerId, ContractLength, ContractSalaryPerWeek, SquadNumber, FansFavoritePlayer, Morale, TransfermarketValue, WantedStarterAppearances, WantedTotalAppearances, Condition, IsSuspendedDomesticCompetition, IsSuspendedContinentalCompetition, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy)
    SELECT s.LeagueTeamId,
           s.PlayerId,
           1 + ((h.Hash % 6) + 6) % 6,
           CASE WHEN p.Wage < 0 THEN 0 ELSE CAST(ROUND(p.Wage, 0) AS BIGINT) END,
           s.SquadNumber,
           0,
           CASE WHEN s.Rating > 90 THEN 5 WHEN s.Rating >= 76 THEN 4 ELSE 3 END,
           CASE WHEN p.[Value] < 0 THEN 0 ELSE CAST(ROUND(p.[Value], 0) AS BIGINT) END,
           0,
           0,
           100,
           0,
           0,
           @CreatedAt,
           @CreatedAt,
           @CreatedBy,
           @CreatedBy
    FROM #Squad s
        INNER JOIN dbo.Players p ON p.Id = s.PlayerId
        CROSS APPLY (SELECT CAST(SUBSTRING(HASHBYTES('SHA2_256', CONCAT(s.LeagueId, ':', s.PlayerId)), 1, 8) AS BIGINT) AS Hash) h;

    DROP TABLE #Used;
    DROP TABLE #Fallback;
    DROP TABLE #Preferred;
    DROP TABLE #Squad;
    DROP TABLE #Taken;
    DROP TABLE #Clubs;
END
