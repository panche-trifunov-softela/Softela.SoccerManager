-- NVARCHAR(100) deliberately matches the application's CreateFormationValidator MaximumLength(100), so the two agree rather than the database silently truncating.
-- UQ_Formations_Name makes the name the formation's business key: a formation is a lookup row, so two rows both named "4-4-2" would be a data-entry mistake rather than two distinct formations.
-- That constraint's index also backs the WHERE Name = @Name lookup in GetFormationByName, which the create and update validators use to report a duplicate as a validation failure instead of a failed insert.
-- The database collation is case-insensitive, so the constraint and that lookup agree on what counts as the same name.
CREATE TABLE dbo.Formations (
    Id         INT              IDENTITY(1,1)    NOT NULL,
    Name       NVARCHAR(100)                     NOT NULL,
    CreatedAt  DATETIME2(7)                      NOT NULL,
    ModifiedAt DATETIME2(7)                      NOT NULL,
    CreatedBy  UNIQUEIDENTIFIER                  NOT NULL,
    ModifiedBy UNIQUEIDENTIFIER                  NOT NULL,
    CONSTRAINT PK_Formations PRIMARY KEY CLUSTERED (Id),
    CONSTRAINT UQ_Formations_Name UNIQUE (Name)
);
