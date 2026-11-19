using FluentValidation;

namespace SoccerManager.Application.Queries.LeagueTeam.GetLeagueTeamTransferBids;

/// <summary>
/// Validates <see cref="GetLeagueTeamTransferBidsRequest"/> instances.
/// </summary>
public sealed class GetLeagueTeamTransferBidsValidator : AbstractValidator<GetLeagueTeamTransferBidsRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetLeagueTeamTransferBidsValidator"/> class.
    /// </summary>
    public GetLeagueTeamTransferBidsValidator()
    {
        // A missing, zero or negative id fails fast here, as a 400, before the access check, where it would be a 403.
        RuleFor(x => x.LeagueTeamId).GreaterThan(0);
    }
}
