using FluentValidation;

namespace SoccerManager.Application.Commands.LeagueTeamPlayerTransferSwap.UpdateLeagueTeamPlayerTransferSwap;

/// <summary>
/// Validates <see cref="UpdateLeagueTeamPlayerTransferSwapRequest"/> instances.
/// </summary>
public sealed class UpdateLeagueTeamPlayerTransferSwapValidator : AbstractValidator<UpdateLeagueTeamPlayerTransferSwapRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateLeagueTeamPlayerTransferSwapValidator"/> class.
    /// </summary>
    public UpdateLeagueTeamPlayerTransferSwapValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.LeagueTeamPlayerTransferId).GreaterThan(0);
        RuleFor(x => x.LeagueTeamPlayerId).GreaterThan(0);
    }
}
