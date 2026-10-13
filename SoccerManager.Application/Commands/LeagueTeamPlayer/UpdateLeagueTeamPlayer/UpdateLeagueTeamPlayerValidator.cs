using FluentValidation;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Commands.LeagueTeamPlayer.UpdateLeagueTeamPlayer;

/// <summary>
/// Validates <see cref="UpdateLeagueTeamPlayerRequest"/> instances.
/// </summary>
public sealed class UpdateLeagueTeamPlayerValidator : AbstractValidator<UpdateLeagueTeamPlayerRequest>
{
    private readonly ILeagueTeamRepository _leagueTeamRepository;
    private readonly ILeagueTeamPlayerRepository _leagueTeamPlayerRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateLeagueTeamPlayerValidator"/> class.
    /// </summary>
    /// <param name="leagueTeamRepository">The repository used to find the league the league team belongs to.</param>
    /// <param name="leagueTeamPlayerRepository">The repository used to check the player has no other club in that league.</param>
    public UpdateLeagueTeamPlayerValidator(ILeagueTeamRepository leagueTeamRepository, ILeagueTeamPlayerRepository leagueTeamPlayerRepository)
    {
        _leagueTeamRepository = leagueTeamRepository;
        _leagueTeamPlayerRepository = leagueTeamPlayerRepository;

        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.LeagueTeamId).GreaterThan(0);
        RuleFor(x => x.PlayerId).GreaterThan(0);

        // No database constraint backs this any more (it needed LeagueId on the row), and the lookup runs
        // outside the handler's transaction, so a race can let a duplicate through. A league team that does
        // not exist passes here on purpose: the foreign key rejects it, so there is no friendly roster check.
        When(x => x.LeagueTeamId > 0 && x.PlayerId > 0, () =>
        {
            RuleFor(x => x.PlayerId)
                .MustAsync((request, _, _) => PlayerHasNoOtherClubInLeagueAsync(request))
                .WithMessage("The player is already in a club in this league.");
        });

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

    /// <summary>
    /// Checks that the player has no registration in the league of the league team apart from the row being updated.
    /// </summary>
    /// <param name="request">The request being validated.</param>
    /// <returns><see langword="true"/> when the league team does not exist or the player's only registration in its league is the row being updated.</returns>
    private async Task<bool> PlayerHasNoOtherClubInLeagueAsync(UpdateLeagueTeamPlayerRequest request)
    {
        var leagueTeam = await _leagueTeamRepository.GetByIdAsync(request.LeagueTeamId);

        if (leagueTeam is null)
        {
            return true;
        }

        var registration = await _leagueTeamPlayerRepository.GetByLeagueAndPlayerAsync(leagueTeam.LeagueId, request.PlayerId);

        // The row itself does not count: updating it may keep the player where they are or move them to
        // another club of the same league.
        return registration is null || registration.Id == request.Id;
    }
}
