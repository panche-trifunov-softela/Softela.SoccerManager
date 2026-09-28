-- NVARCHAR(100) deliberately matches the application's CreateNationalTeamValidator MaximumLength(100), and NVARCHAR(500) does the same for JerseyUrl/LogoUrl, so the two agree rather than the database silently truncating.
-- Name is deliberately not unique, following dbo.Teams, even though national teams are naturally unique.
-- StadiumId is nullable because a national team never needs a stadium. FK_NationalTeams_Stadiums is declared inline, since dbo.Stadiums already exists (V1_0_0_12), and uses ON DELETE SET NULL like FK_Teams_Stadiums, so deleting a stadium un-assigns it instead of being blocked.
-- JerseyUrl/LogoUrl hold URLs only; no image bytes are stored here, upload is deferred.
CREATE TABLE dbo.NationalTeams (
    Id         INT              IDENTITY(1,1)    NOT NULL,
    Name       NVARCHAR(100)                     NOT NULL,
    StadiumId  INT                                   NULL,
    JerseyUrl  NVARCHAR(500)                         NULL,
    LogoUrl    NVARCHAR(500)                         NULL,
    CreatedAt  DATETIME2(7)                      NOT NULL,
    ModifiedAt DATETIME2(7)                      NOT NULL,
    CreatedBy  UNIQUEIDENTIFIER                  NOT NULL,
    ModifiedBy UNIQUEIDENTIFIER                  NOT NULL,
    CONSTRAINT PK_NationalTeams PRIMARY KEY CLUSTERED (Id),
    CONSTRAINT FK_NationalTeams_Stadiums FOREIGN KEY (StadiumId) REFERENCES dbo.Stadiums (Id) ON DELETE SET NULL
);
