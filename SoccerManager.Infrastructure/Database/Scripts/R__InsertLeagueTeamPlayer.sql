CREATE OR ALTER PROCEDURE dbo.InsertLeagueTeamPlayer
    @LeagueTeamId                      INT,
    @PlayerId                          INT,
    @ContractLength                    INT,
    @ContractSalaryPerWeek             BIGINT,
    @SquadNumber                       INT,
    @FansFavoritePlayer                BIT,
    @Morale                            TINYINT,
    @TransfermarketValue               BIGINT,
    @WantedStarterAppearances          INT,
    @WantedTotalAppearances            INT,
    @Condition                         INT,
    @IsSuspendedDomesticCompetition    BIT,
    @IsSuspendedContinentalCompetition BIT,
    @CreatedAt                         DATETIME2(7),
    @ModifiedAt                        DATETIME2(7),
    @CreatedBy                         UNIQUEIDENTIFIER,
    @ModifiedBy                        UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    -- A national team is on a League's roster to play its national competition (V1_0_0_68); a national squad waits for national team management.
    IF EXISTS (SELECT 1 FROM dbo.LeagueTeams WHERE Id = @LeagueTeamId AND NationalTeamId IS NOT NULL)
        THROW 50000, 'A national team has no squad registrations.', 1;

    INSERT INTO dbo.LeagueTeamPlayers (LeagueTeamId, PlayerId, ContractLength, ContractSalaryPerWeek, SquadNumber, FansFavoritePlayer, Morale, TransfermarketValue, WantedStarterAppearances, WantedTotalAppearances, Condition, IsSuspendedDomesticCompetition, IsSuspendedContinentalCompetition, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy)
    OUTPUT INSERTED.Id
    VALUES (@LeagueTeamId, @PlayerId, @ContractLength, @ContractSalaryPerWeek, @SquadNumber, @FansFavoritePlayer, @Morale, @TransfermarketValue, @WantedStarterAppearances, @WantedTotalAppearances, @Condition, @IsSuspendedDomesticCompetition, @IsSuspendedContinentalCompetition, @CreatedAt, @ModifiedAt, @CreatedBy, @ModifiedBy);
END
