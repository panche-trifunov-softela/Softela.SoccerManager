using FluentValidation;

namespace SoccerManager.Application.Commands.LeagueTeamPlayerTransferSwap.CreateLeagueTeamPlayerTransferSwap;

/// <summary>
/// Validates <see cref="CreateLeagueTeamPlayerTransferSwapRequest"/> instances.
/// </summary>
public sealed class CreateLeagueTeamPlayerTransferSwapValidator : AbstractValidator<CreateLeagueTeamPlayerTransferSwapRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateLeagueTeamPlayerTransferSwapValidator"/> class.
    /// </summary>
    public CreateLeagueTeamPlayerTransferSwapValidator()
    {
        RuleFor(x => x.LeagueTeamPlayerTransferId).GreaterThan(0);
        RuleFor(x => x.LeagueTeamPlayerId).GreaterThan(0);
    }
}
