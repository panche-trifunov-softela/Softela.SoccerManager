using FluentValidation;

namespace SoccerManager.Application.Queries.LeagueTeamPlayerTransfer.GetLeagueTeamPlayerTransfers;

/// <summary>
/// Validates <see cref="GetLeagueTeamPlayerTransfersRequest"/> instances.
/// </summary>
public sealed class GetLeagueTeamPlayerTransfersValidator : AbstractValidator<GetLeagueTeamPlayerTransfersRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetLeagueTeamPlayerTransfersValidator"/> class.
    /// </summary>
    public GetLeagueTeamPlayerTransfersValidator()
    {
        // This query is always scoped to a squad row, so a missing or zero
        // LeagueTeamPlayerId must fail fast here rather than silently returning nothing.
        RuleFor(x => x.LeagueTeamPlayerId).GreaterThan(0);
    }
}
