-- Name NVARCHAR(100) deliberately matches the application's CreateStadiumValidator MaximumLength(100), so the two agree rather than the database silently truncating.
-- ImageUrl NVARCHAR(500) matches MaximumLength(500); it holds a URL only, no image bytes.
-- Size is the capacity in spectators, and CK_Stadiums_SizePositive mirrors the validator's GreaterThan(0), so the two agree.
-- FK_Teams_Stadiums is the constraint V1_0_0_04 deferred until this table existed. It uses ON DELETE SET NULL rather than CASCADE because Teams.StadiumId was designed nullable, so a deleted stadium simply un-assigns itself from its teams; SET NULL touches only Teams rows, so it never reaches the LeagueTeamManagers cascade.
-- The UPDATE below clears every existing StadiumId, because no value stored before this migration ever referred to a real stadium, and adding the foreign key with the default WITH CHECK would otherwise fail on such a row.
CREATE TABLE dbo.Stadiums (
    Id         INT              IDENTITY(1,1)    NOT NULL,
    Name       NVARCHAR(100)                     NOT NULL,
    ImageUrl   NVARCHAR(500)                         NULL,
    Size       INT                               NOT NULL,
    CreatedAt  DATETIME2(7)                      NOT NULL,
    ModifiedAt DATETIME2(7)                      NOT NULL,
    CreatedBy  UNIQUEIDENTIFIER                  NOT NULL,
    ModifiedBy UNIQUEIDENTIFIER                  NOT NULL,
    CONSTRAINT PK_Stadiums PRIMARY KEY CLUSTERED (Id),
    CONSTRAINT CK_Stadiums_SizePositive CHECK (Size > 0)
);

UPDATE dbo.Teams SET StadiumId = NULL WHERE StadiumId IS NOT NULL;

ALTER TABLE dbo.Teams
    ADD CONSTRAINT FK_Teams_Stadiums FOREIGN KEY (StadiumId) REFERENCES dbo.Stadiums (Id) ON DELETE SET NULL;
