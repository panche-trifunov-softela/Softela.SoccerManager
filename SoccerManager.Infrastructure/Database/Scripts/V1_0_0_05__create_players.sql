-- NVARCHAR(100) deliberately matches the application's CreatePlayerValidator MaximumLength(100), so the two agree rather than the database silently truncating.
-- DateOfBirth is DATE, not DATETIME2, so it carries no time and no zone, keeping a birth date free of the UTC stamping the audit columns get.
-- [Value] is bracketed because VALUE is an ODBC reserved word that Microsoft recommends avoiding as an identifier.
-- ImageUrl holds a URL only; no image bytes are stored here, upload is deferred.
CREATE TABLE dbo.Players (
    Id          INT              IDENTITY(1,1)    NOT NULL,
    Name        NVARCHAR(100)                     NOT NULL,
    DateOfBirth DATE                              NOT NULL,
    Rating      INT                               NOT NULL,
    [Value]     DECIMAL(18,2)                     NOT NULL,
    Wage        DECIMAL(18,2)                     NOT NULL,
    ImageUrl    NVARCHAR(500)                         NULL,
    CreatedAt   DATETIME2(7)                      NOT NULL,
    ModifiedAt  DATETIME2(7)                      NOT NULL,
    CreatedBy   UNIQUEIDENTIFIER                  NOT NULL,
    ModifiedBy  UNIQUEIDENTIFIER                  NOT NULL,
    CONSTRAINT PK_Players PRIMARY KEY CLUSTERED (Id)
);
