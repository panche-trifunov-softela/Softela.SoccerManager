CREATE OR ALTER PROCEDURE dbo.UpdateLeagueTeamPlayer
    @Id                                INT,
    @LeagueId                          INT,
    @TeamId                            INT,
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

    UPDATE dbo.LeagueTeamPlayers
    SET LeagueId                          = @LeagueId,
        TeamId                            = @TeamId,
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
