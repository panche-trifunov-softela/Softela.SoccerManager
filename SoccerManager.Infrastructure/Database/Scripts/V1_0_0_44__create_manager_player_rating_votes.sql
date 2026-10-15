-- A manager's up or down vote on a player's rating. The server copies RatingAtTheMoment from
-- Players.Rating when the vote is cast, so the unique key means one vote per manager, player and
-- rating: a manager can vote again once the rating changes, and the old votes stay as history.
-- No CHECK on RatingAtTheMoment, since Players.Rating has none either.
-- Both foreign keys cascade. Managers has no parent tables, and Players is reached only through
-- SET NULL, which ends a cascade there, so there is no multiple-cascade-path conflict.
-- The unique key leads with ManagerId and so also serves that foreign key; PlayerId gets its own index.
CREATE TABLE dbo.ManagerPlayerRatingVotes
(
    Id                   INT                 IDENTITY(1,1)    NOT NULL,
    ManagerId            INT                                  NOT NULL,
    PlayerId             INT                                  NOT NULL,
    RatingAtTheMoment    INT                                  NOT NULL,
    UpOrDown             BIT                                  NOT NULL,
    CreatedAt            DATETIME2(7)                         NOT NULL,
    ModifiedAt           DATETIME2(7)                         NOT NULL,
    CreatedBy            UNIQUEIDENTIFIER                     NOT NULL,
    ModifiedBy           UNIQUEIDENTIFIER                     NOT NULL,
    CONSTRAINT PK_ManagerPlayerRatingVotes PRIMARY KEY CLUSTERED (Id),
    CONSTRAINT FK_ManagerPlayerRatingVotes_Managers FOREIGN KEY (ManagerId) REFERENCES dbo.Managers (Id) ON DELETE CASCADE,
    CONSTRAINT FK_ManagerPlayerRatingVotes_Players FOREIGN KEY (PlayerId) REFERENCES dbo.Players (Id) ON DELETE CASCADE,
    CONSTRAINT UQ_ManagerPlayerRatingVotes_ManagerId_PlayerId_RatingAtTheMoment UNIQUE (ManagerId, PlayerId, RatingAtTheMoment)
);

CREATE NONCLUSTERED INDEX IX_ManagerPlayerRatingVotes_PlayerId ON dbo.ManagerPlayerRatingVotes (PlayerId);
