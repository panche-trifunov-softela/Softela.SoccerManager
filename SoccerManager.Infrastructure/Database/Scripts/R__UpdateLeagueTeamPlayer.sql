CREATE OR ALTER PROCEDURE dbo.UpdateLeagueTeamPlayer
    @Id                                INT,
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
    @ModifiedAt                        DATETIME2(7),
    @ModifiedBy                        UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    -- A national team is on a League's roster to play its national competition (V1_0_0_68); a national squad waits for national team management.
    IF EXISTS (SELECT 1 FROM dbo.LeagueTeams WHERE Id = @LeagueTeamId AND NationalTeamId IS NOT NULL)
        THROW 50000, 'A national team has no squad registrations.', 1;

    UPDATE dbo.LeagueTeamPlayers
    SET LeagueTeamId                      = @LeagueTeamId,
        PlayerId                          = @PlayerId,
        ContractLength                    = @ContractLength,
        ContractSalaryPerWeek             = @ContractSalaryPerWeek,
        SquadNumber                       = @SquadNumber,
        FansFavoritePlayer                = @FansFavoritePlayer,
        Morale                            = @Morale,
        TransfermarketValue               = @TransfermarketValue,
        WantedStarterAppearances          = @WantedStarterAppearances,
        WantedTotalAppearances            = @WantedTotalAppearances,
        Condition                         = @Condition,
        IsSuspendedDomesticCompetition    = @IsSuspendedDomesticCompetition,
        IsSuspendedContinentalCompetition = @IsSuspendedContinentalCompetition,
        ModifiedAt                        = @ModifiedAt,
        ModifiedBy                        = @ModifiedBy
    WHERE Id = @Id;

    SELECT @Id;
END
