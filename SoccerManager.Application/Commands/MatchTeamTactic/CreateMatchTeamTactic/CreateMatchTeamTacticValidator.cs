using FluentValidation;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Commands.MatchTeamTactic.CreateMatchTeamTactic;

/// <summary>
/// Validates <see cref="CreateMatchTeamTacticRequest"/> instances.
/// </summary>
public sealed class CreateMatchTeamTacticValidator : AbstractValidator<CreateMatchTeamTacticRequest>
{
    private readonly ILeagueTeamRepository _leagueTeamRepository;
    private readonly ILeagueTeamPlayerRepository _leagueTeamPlayerRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateMatchTeamTacticValidator"/> class.
    /// </summary>
    /// <param name="leagueTeamRepository">The repository used to find the league the club plays in.</param>
    /// <param name="leagueTeamPlayerRepository">The repository used to check each named player is in the club's squad.</param>
    public CreateMatchTeamTacticValidator(ILeagueTeamRepository leagueTeamRepository, ILeagueTeamPlayerRepository leagueTeamPlayerRepository)
    {
        _leagueTeamRepository = leagueTeamRepository;
        _leagueTeamPlayerRepository = leagueTeamPlayerRepository;

        RuleFor(x => x.MatchId).GreaterThan(0);
        RuleFor(x => x.LeagueTeamId).GreaterThan(0);

        // Every enum starts at 1, so IsInEnum also rejects an omitted or default zero.
        RuleFor(x => x.Mentality).IsInEnum();
        RuleFor(x => x.Tempo).IsInEnum();
        RuleFor(x => x.Passing).IsInEnum();
        RuleFor(x => x.Width).IsInEnum();
        RuleFor(x => x.Pressing).IsInEnum();
        RuleFor(x => x.Tackling).IsInEnum();
        RuleFor(x => x.AttackingSide).IsInEnum();

        RuleFor(x => x.PenaltyTakerPlayerId).GreaterThan(0);
        RuleFor(x => x.FreeKickTakerPlayerId).GreaterThan(0);
        RuleFor(x => x.CornerKickTakerPlayerId).GreaterThan(0);
        RuleFor(x => x.CaptainPlayerId).GreaterThan(0);

        // The lookups run outside the handler's transaction. A club that does not exist passes here on
        // purpose: the stored procedure and the foreign key reject it, so there is no friendly squad check.
        // One player may hold several roles, so each role is checked on its own.
        When(x => x.LeagueTeamId > 0 && x.CaptainPlayerId > 0, () =>
        {
            RuleFor(x => x.CaptainPlayerId)
                .MustAsync((request, _, _) => PlayerIsInClubSquadAsync(request.LeagueTeamId, request.CaptainPlayerId))
                .WithMessage("The captain must be in the club's squad.");
        });

        When(x => x.LeagueTeamId > 0 && x.PenaltyTakerPlayerId > 0, () =>
        {
            RuleFor(x => x.PenaltyTakerPlayerId)
                .MustAsync((request, _, _) => PlayerIsInClubSquadAsync(request.LeagueTeamId, request.PenaltyTakerPlayerId))
                .WithMessage("The penalty taker must be in the club's squad.");
        });

        When(x => x.LeagueTeamId > 0 && x.FreeKickTakerPlayerId > 0, () =>
        {
            RuleFor(x => x.FreeKickTakerPlayerId)
                .MustAsync((request, _, _) => PlayerIsInClubSquadAsync(request.LeagueTeamId, request.FreeKickTakerPlayerId))
                .WithMessage("The free-kick taker must be in the club's squad.");
        });

        When(x => x.LeagueTeamId > 0 && x.CornerKickTakerPlayerId > 0, () =>
        {
            RuleFor(x => x.CornerKickTakerPlayerId)
                .MustAsync((request, _, _) => PlayerIsInClubSquadAsync(request.LeagueTeamId, request.CornerKickTakerPlayerId))
                .WithMessage("The corner-kick taker must be in the club's squad.");
        });
    }

    /// <summary>
    /// Checks that the player is registered with the given club in the club's league.
    /// </summary>
    /// <param name="leagueTeamId">The identifier of the league roster entry (the club in its league).</param>
    /// <param name="playerId">The identifier of the player.</param>
    /// <returns><see langword="true"/> when the club does not exist or the player's registration in its league is with the club.</returns>
    private async Task<bool> PlayerIsInClubSquadAsync(int leagueTeamId, int playerId)
    {
        var leagueTeam = await _leagueTeamRepository.GetByIdAsync(leagueTeamId);

        if (leagueTeam is null)
        {
            return true;
        }

        var registration = await _leagueTeamPlayerRepository.GetByLeagueAndPlayerAsync(leagueTeam.LeagueId, playerId);

        return registration is not null && registration.LeagueTeamId == leagueTeamId;
    }
}
