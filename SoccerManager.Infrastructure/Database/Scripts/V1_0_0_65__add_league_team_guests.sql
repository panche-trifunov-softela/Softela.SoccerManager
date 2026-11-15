-- V1_0_0_36, V1_0_0_38 and V1_0_0_39 are already applied and are never edited; Evolve rejects a changed checksum.
-- LeagueTeams.IsGuest marks a club that is on a League's roster so its players can be bought, but is never managed and never plays in a division. Every existing roster row is a league club, so it takes the default 0.
-- The guest lock: GuestMarker on LeagueTeamManagers and LeagueTeamManagerApplications is always 0 (CHECK), so the composite foreign key (LeagueTeamId, GuestMarker) to LeagueTeams (Id, IsGuest) only accepts a roster row with IsGuest = 0. It is NO ACTION and adds no cascade path; the existing cascading LeagueTeamId foreign keys still remove these rows with their club.
-- The backfill gives every League that already has a roster every Team not on it as a guest, plus default squads for those new guests. The squad block repeats dbo.InsertDefaultLeagueTeamPlayers, because Evolve runs every V script before the R__ scripts.
-- GO is required: SQL Server compiles a whole batch before running it, so the keys and the backfill cannot name columns the same batch adds (V1_0_0_15). Evolve still runs all batches in one transaction.
ALTER TABLE dbo.LeagueTeams ADD IsGuest BIT NOT NULL CONSTRAINT DF_LeagueTeams_IsGuest DEFAULT 0;

ALTER TABLE dbo.LeagueTeamManagers ADD GuestMarker BIT NOT NULL
    CONSTRAINT DF_LeagueTeamManagers_GuestMarker DEFAULT 0
    CONSTRAINT CK_LeagueTeamManagers_GuestMarker CHECK (GuestMarker = 0);

ALTER TABLE dbo.LeagueTeamManagerApplications ADD GuestMarker BIT NOT NULL
    CONSTRAINT DF_LeagueTeamManagerApplications_GuestMarker DEFAULT 0
    CONSTRAINT CK_LeagueTeamManagerApplications_GuestMarker CHECK (GuestMarker = 0);
GO

ALTER TABLE dbo.LeagueTeams ADD CONSTRAINT UQ_LeagueTeams_Id_IsGuest UNIQUE (Id, IsGuest);

ALTER TABLE dbo.LeagueTeamManagers ADD CONSTRAINT FK_LeagueTeamManagers_LeagueTeams_IsGuest
    FOREIGN KEY (LeagueTeamId, GuestMarker) REFERENCES dbo.LeagueTeams (Id, IsGuest);

ALTER TABLE dbo.LeagueTeamManagerApplications ADD CONSTRAINT FK_LeagueTeamManagerApplications_LeagueTeams_IsGuest
    FOREIGN KEY (LeagueTeamId, GuestMarker) REFERENCES dbo.LeagueTeams (Id, IsGuest);
GO

DECLARE @Now DATETIME2(7) = SYSUTCDATETIME();
DECLARE @ImportUserId UNIQUEIDENTIFIER = '00000000-0000-0000-0000-000000000001';

-- Every Team not yet on the roster of a League that has one becomes a guest of it.
DECLARE @NewGuests TABLE (LeagueTeamId INT NOT NULL PRIMARY KEY, LeagueId INT NOT NULL, TeamId INT NOT NULL);

INSERT INTO dbo.LeagueTeams (LeagueId, TeamId, IsGuest, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy)
OUTPUT INSERTED.Id, INSERTED.LeagueId, INSERTED.TeamId INTO @NewGuests (LeagueTeamId, LeagueId, TeamId)
SELECT l.Id, t.Id, 1, @Now, @Now, @ImportUserId, @ImportUserId
FROM dbo.Leagues l
    CROSS JOIN dbo.Teams t
WHERE EXISTS (SELECT 1 FROM dbo.LeagueTeams lt WHERE lt.LeagueId = l.Id)
  AND NOT EXISTS (SELECT 1 FROM dbo.LeagueTeams lt WHERE lt.LeagueId = l.Id AND lt.TeamId = t.Id);

-- SQUAD BLOCK: identical to the one in R__InsertDefaultLeagueTeamPlayers.sql; only the input set and the audit values differ.
-- Players already on a squad of the League: the one-club-per-League rule has no database key since V1_0_0_37.
CREATE TABLE #Taken (LeagueId INT NOT NULL, PlayerId INT NOT NULL, PRIMARY KEY (LeagueId, PlayerId));

INSERT INTO #Taken (LeagueId, PlayerId)
SELECT DISTINCT lt.LeagueId, ltp.PlayerId
FROM dbo.LeagueTeamPlayers ltp
    INNER JOIN dbo.LeagueTeams lt ON lt.Id = ltp.LeagueTeamId
WHERE lt.LeagueId IN (SELECT LeagueId FROM @NewGuests);

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
FROM @NewGuests c
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
       @Now,
       @Now,
       @ImportUserId,
       @ImportUserId
FROM #Squad s
    INNER JOIN dbo.Players p ON p.Id = s.PlayerId
    CROSS APPLY (SELECT CAST(SUBSTRING(HASHBYTES('SHA2_256', CONCAT(s.LeagueId, ':', s.PlayerId)), 1, 8) AS BIGINT) AS Hash) h;

DROP TABLE #Used;
DROP TABLE #Fallback;
DROP TABLE #Preferred;
DROP TABLE #Squad;
DROP TABLE #Taken;
