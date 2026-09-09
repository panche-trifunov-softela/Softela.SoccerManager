-- NVARCHAR(100) deliberately matches the application's CreatePositionValidator MaximumLength(100), so the two agree rather than the database silently truncating.
-- Area is TINYINT holding the PositionArea enum value, where 1 is Attack and 4 is Goalkeeper.
-- Side is TINYINT holding the PositionSide enum value, where 1 is Left and 3 is Right.
-- Both enums deliberately start at 1, so a value that was never set is rejected rather than stored as the first member.
CREATE TABLE dbo.Positions (
    Id         INT              IDENTITY(1,1)    NOT NULL,
    Name       NVARCHAR(100)                     NOT NULL,
    Area       TINYINT                           NOT NULL,
    Side       TINYINT                           NOT NULL,
    CreatedAt  DATETIME2(7)                      NOT NULL,
    ModifiedAt DATETIME2(7)                      NOT NULL,
    CreatedBy  UNIQUEIDENTIFIER                  NOT NULL,
    ModifiedBy UNIQUEIDENTIFIER                  NOT NULL,
    CONSTRAINT PK_Positions PRIMARY KEY CLUSTERED (Id)
);
