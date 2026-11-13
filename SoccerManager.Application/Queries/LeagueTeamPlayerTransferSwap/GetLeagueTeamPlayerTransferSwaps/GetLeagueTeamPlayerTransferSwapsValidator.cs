using FluentValidation;

namespace SoccerManager.Application.Queries.LeagueTeamPlayerTransferSwap.GetLeagueTeamPlayerTransferSwaps;

/// <summary>
/// Validates <see cref="GetLeagueTeamPlayerTransferSwapsRequest"/> instances.
/// </summary>
public sealed class GetLeagueTeamPlayerTransferSwapsValidator : AbstractValidator<GetLeagueTeamPlayerTransferSwapsRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetLeagueTeamPlayerTransferSwapsValidator"/> class.
    /// </summary>
    public GetLeagueTeamPlayerTransferSwapsValidator()
    {
        // This query is always scoped to a transfer, so a missing or zero
        // LeagueTeamPlayerTransferId must fail fast here rather than silently returning nothing.
        RuleFor(x => x.LeagueTeamPlayerTransferId).GreaterThan(0);
    }
}
