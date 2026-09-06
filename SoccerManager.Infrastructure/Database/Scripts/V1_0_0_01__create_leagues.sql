-- NVARCHAR(100) deliberately matches the application's CreateLeagueValidator MaximumLength(100), so the two agree rather than the database silently truncating.
CREATE TABLE dbo.Leagues (
    Id         INT              IDENTITY(1,1)    NOT NULL,
    Name       NVARCHAR(100)                     NOT NULL,
    CreatedAt  DATETIME2(7)                      NOT NULL,
    ModifiedAt DATETIME2(7)                      NOT NULL,
    CreatedBy  UNIQUEIDENTIFIER                  NOT NULL,
    ModifiedBy UNIQUEIDENTIFIER                  NOT NULL,
    CONSTRAINT PK_Leagues PRIMARY KEY CLUSTERED (Id)
);
