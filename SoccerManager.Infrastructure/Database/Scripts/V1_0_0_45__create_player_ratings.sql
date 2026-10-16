-- Moves the player rating into its own history table, so a vote belongs to one rating round even when a
-- player's rating later returns to an earlier value: 80, then 79, then 80 again are three rows.
-- dbo.PlayerRatings keeps every rating a player has had. IsCurrent marks the one in force, and
-- UX_PlayerRatings_PlayerId_Current allows one current row per player; dbo.InsertPlayerRating retires the
-- old current row before it adds the new one. dbo.Players.Rating is dropped, and reads join the current row,
-- so every player must keep exactly one current rating: a script that adds a player or changes a rating by
-- hand must write dbo.PlayerRatings the way dbo.InsertPlayerRating does.
-- A vote now points at the rating it was cast on (PlayerRatingId) instead of carrying PlayerId and
-- RatingAtTheMoment. The player is reached through the rating, so the only cascade into the votes from
-- dbo.Players runs through dbo.PlayerRatings, and FK_ManagerPlayerRatingVotes_Managers is unchanged.
-- Backfill, stamped like the V1_0_0_36 backfill: every player's rating becomes its current row; a vote cast
-- at the player's current rating joins that row, and a vote cast at an earlier rating gets a non-current row
-- for that player and rating, dated by its earliest vote.
DECLARE @Now DATETIME2(7) = SYSUTCDATETIME();
DECLARE @ImportUserId UNIQUEIDENTIFIER = '00000000-0000-0000-0000-000000000001';

CREATE TABLE dbo.PlayerRatings
(
    Id           INT                 IDENTITY(1,1)    NOT NULL,
    PlayerId     INT                                  NOT NULL,
    Rating       INT                                  NOT NULL,
    IsCurrent    BIT                                  NOT NULL,
    CreatedAt    DATETIME2(7)                         NOT NULL,
    ModifiedAt   DATETIME2(7)                         NOT NULL,
    CreatedBy    UNIQUEIDENTIFIER                     NOT NULL,
    ModifiedBy   UNIQUEIDENTIFIER                     NOT NULL,
    CONSTRAINT PK_PlayerRatings PRIMARY KEY CLUSTERED (Id),
    CONSTRAINT FK_PlayerRatings_Players FOREIGN KEY (PlayerId) REFERENCES dbo.Players (Id) ON DELETE CASCADE
);

CREATE NONCLUSTERED INDEX IX_PlayerRatings_PlayerId ON dbo.PlayerRatings (PlayerId);

CREATE UNIQUE NONCLUSTERED INDEX UX_PlayerRatings_PlayerId_Current ON dbo.PlayerRatings (PlayerId) WHERE IsCurrent = 1;

INSERT INTO dbo.PlayerRatings (PlayerId, Rating, IsCurrent, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy)
SELECT p.Id, p.Rating, 1, @Now, @Now, @ImportUserId, @ImportUserId
FROM dbo.Players p;

ALTER TABLE dbo.ManagerPlayerRatingVotes ADD PlayerRatingId INT NULL;
GO

DECLARE @Now DATETIME2(7) = SYSUTCDATETIME();
DECLARE @ImportUserId UNIQUEIDENTIFIER = '00000000-0000-0000-0000-000000000001';

INSERT INTO dbo.PlayerRatings (PlayerId, Rating, IsCurrent, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy)
SELECT v.PlayerId, v.RatingAtTheMoment, 0, MIN(v.CreatedAt), @Now, @ImportUserId, @ImportUserId
FROM dbo.ManagerPlayerRatingVotes v
    INNER JOIN dbo.Players p ON p.Id = v.PlayerId
WHERE v.RatingAtTheMoment <> p.Rating
GROUP BY v.PlayerId, v.RatingAtTheMoment;

UPDATE v
SET PlayerRatingId = pr.Id
FROM dbo.ManagerPlayerRatingVotes v
    INNER JOIN dbo.Players p ON p.Id = v.PlayerId
    INNER JOIN dbo.PlayerRatings pr ON pr.PlayerId = v.PlayerId
        AND pr.Rating = v.RatingAtTheMoment
        AND pr.IsCurrent = CASE WHEN v.RatingAtTheMoment = p.Rating THEN 1 ELSE 0 END;
GO

ALTER TABLE dbo.ManagerPlayerRatingVotes ALTER COLUMN PlayerRatingId INT NOT NULL;

ALTER TABLE dbo.ManagerPlayerRatingVotes DROP CONSTRAINT FK_ManagerPlayerRatingVotes_Players;
ALTER TABLE dbo.ManagerPlayerRatingVotes DROP CONSTRAINT UQ_ManagerPlayerRatingVotes_ManagerId_PlayerId_RatingAtTheMoment;
DROP INDEX IX_ManagerPlayerRatingVotes_PlayerId ON dbo.ManagerPlayerRatingVotes;
ALTER TABLE dbo.ManagerPlayerRatingVotes DROP COLUMN PlayerId, RatingAtTheMoment;

ALTER TABLE dbo.ManagerPlayerRatingVotes
    ADD CONSTRAINT FK_ManagerPlayerRatingVotes_PlayerRatings FOREIGN KEY (PlayerRatingId) REFERENCES dbo.PlayerRatings (Id) ON DELETE CASCADE,
        CONSTRAINT UQ_ManagerPlayerRatingVotes_ManagerId_PlayerRatingId UNIQUE (ManagerId, PlayerRatingId);

CREATE NONCLUSTERED INDEX IX_ManagerPlayerRatingVotes_PlayerRatingId ON dbo.ManagerPlayerRatingVotes (PlayerRatingId);

ALTER TABLE dbo.Players DROP COLUMN Rating;

-- Replaced by dbo.GetManagerPlayerRatingVoteByManagerAndPlayerRating; versioned scripts run before the
-- repeatable ones, so IF EXISTS keeps a fresh database working.
DROP PROCEDURE IF EXISTS dbo.GetManagerPlayerRatingVoteByManagerPlayerAndRating;
