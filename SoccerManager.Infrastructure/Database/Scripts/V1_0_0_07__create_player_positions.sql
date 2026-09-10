-- Quality is constrained to 1-100 in the database as well as in the validators, so the two agree rather than the database accepting a rating the application would reject.
-- UQ_PlayerPositions_PlayerId_PositionId also serves as the index backing the WHERE PlayerId = @PlayerId lookup in GetPlayerPositions, so no separate index is needed.
-- Both foreign keys cascade: Players and Positions are independent root tables with no foreign keys of their own, so there is no cycle and no shared ancestor for SQL Server to reject.
CREATE TABLE dbo.PlayerPositions (
    Id         INT              IDENTITY(1,1)    NOT NULL,
    PlayerId   INT                               NOT NULL,
    PositionId INT                               NOT NULL,
    Quality    INT                               NOT NULL,
    CreatedAt  DATETIME2(7)                      NOT NULL,
    ModifiedAt DATETIME2(7)                      NOT NULL,
    CreatedBy  UNIQUEIDENTIFIER                  NOT NULL,
    ModifiedBy UNIQUEIDENTIFIER                  NOT NULL,
    CONSTRAINT PK_PlayerPositions PRIMARY KEY CLUSTERED (Id),
    CONSTRAINT FK_PlayerPositions_Players FOREIGN KEY (PlayerId) REFERENCES dbo.Players (Id) ON DELETE CASCADE,
    CONSTRAINT FK_PlayerPositions_Positions FOREIGN KEY (PositionId) REFERENCES dbo.Positions (Id) ON DELETE CASCADE,
    CONSTRAINT UQ_PlayerPositions_PlayerId_PositionId UNIQUE (PlayerId, PositionId),
    CONSTRAINT CK_PlayerPositions_Quality CHECK (Quality BETWEEN 1 AND 100)
);
