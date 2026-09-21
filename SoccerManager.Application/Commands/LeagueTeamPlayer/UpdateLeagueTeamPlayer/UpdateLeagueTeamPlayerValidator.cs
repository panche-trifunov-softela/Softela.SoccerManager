using FluentValidation;

namespace SoccerManager.Application.Commands.LeagueTeamPlayer.UpdateLeagueTeamPlayer;

/// <summary>
/// Validates <see cref="UpdateLeagueTeamPlayerRequest"/> instances.
/// </summary>
public sealed class UpdateLeagueTeamPlayerValidator : AbstractValidator<UpdateLeagueTeamPlayerRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateLeagueTeamPlayerValidator"/> class.
    /// </summary>
    public UpdateLeagueTeamPlayerValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.LeagueId).GreaterThan(0);
        RuleFor(x => x.TeamId).GreaterThan(0);
        RuleFor(x => x.PlayerId).GreaterThan(0);

        // Both ranges match the CHECK constraints on dbo.LeagueTeamPlayers, so the two agree.
        RuleFor(x => x.ContractLength).InclusiveBetween(1, 6);
        RuleFor(x => x.SquadNumber).InclusiveBetween(1, 99);

        RuleFor(x => x.ContractSalaryPerWeek).GreaterThanOrEqualTo(0);
        RuleFor(x => x.TransfermarketValue).GreaterThanOrEqualTo(0);

        RuleFor(x => x.Morale).IsInEnum();

        RuleFor(x => x.WantedStarterAppearances).GreaterThanOrEqualTo(0);
        RuleFor(x => x.WantedTotalAppearances).GreaterThanOrEqualTo(0);

        // Starting appearances are appearances too, so the wanted total can never be lower than them.
        RuleFor(x => x.WantedTotalAppearances).GreaterThanOrEqualTo(x => x.WantedStarterAppearances);

        RuleFor(x => x.Condition).InclusiveBetween(1, 100);
    }
}
