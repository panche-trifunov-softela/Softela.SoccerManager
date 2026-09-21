-- SlotNumber is constrained to 1-11 in the database as well as in the validators, so the two agree rather than the database accepting a slot the application would reject. It is INT rather than TINYINT to match Quality, SquadNumber and ContractLength, the repo's other bounded non-enum numbers; TINYINT here is reserved for enum-backed columns.
-- There is deliberately NO unique constraint on (FormationId, PositionId): a formation uses the same position more than once, since a 4-4-2 fields two centre defenders, two centre midfielders and two centre attackers. The slot, not the position, is what must be unique within a formation.
-- UQ_FormationPositions_FormationId_SlotNumber also serves as the index backing the WHERE FormationId = @FormationId lookup in GetFormationPositions and the cascade from Formations, so no separate index is needed for either.
-- IX_FormationPositions_PositionId backs the cascade from Positions, which that unique constraint's leading FormationId does not cover.
-- Both foreign keys cascade: Formations and Positions are independent root tables with no foreign keys of their own, so there is no cycle and no shared ancestor for SQL Server to reject.
CREATE TABLE dbo.FormationPositions (
    Id          INT              IDENTITY(1,1)    NOT NULL,
    FormationId INT                               NOT NULL,
    PositionId  INT                               NOT NULL,
    SlotNumber  INT                               NOT NULL,
    CreatedAt   DATETIME2(7)                      NOT NULL,
    ModifiedAt  DATETIME2(7)                      NOT NULL,
    CreatedBy   UNIQUEIDENTIFIER                  NOT NULL,
    ModifiedBy  UNIQUEIDENTIFIER                  NOT NULL,
    CONSTRAINT PK_FormationPositions PRIMARY KEY CLUSTERED (Id),
    CONSTRAINT FK_FormationPositions_Formations FOREIGN KEY (FormationId) REFERENCES dbo.Formations (Id) ON DELETE CASCADE,
    CONSTRAINT FK_FormationPositions_Positions FOREIGN KEY (PositionId) REFERENCES dbo.Positions (Id) ON DELETE CASCADE,
    CONSTRAINT UQ_FormationPositions_FormationId_SlotNumber UNIQUE (FormationId, SlotNumber),
    CONSTRAINT CK_FormationPositions_SlotNumber CHECK (SlotNumber BETWEEN 1 AND 11)
);

CREATE NONCLUSTERED INDEX IX_FormationPositions_PositionId ON dbo.FormationPositions (PositionId);
