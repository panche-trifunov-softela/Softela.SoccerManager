using FluentValidation;

namespace SoccerManager.Application.Queries.LeagueTeam.GetLeaguePlayerBidders;

/// <summary>
/// Validates <see cref="GetLeaguePlayerBiddersRequest"/> instances.
/// </summary>
public sealed class GetLeaguePlayerBiddersValidator : AbstractValidator<GetLeaguePlayerBiddersRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetLeaguePlayerBiddersValidator"/> class.
    /// </summary>
    public GetLeaguePlayerBiddersValidator()
    {
        // A missing, zero or negative id fails fast here, as a 400, before the access check and the lookup, where it would be
        // a 403 or a 404.
        RuleFor(x => x.LeagueTeamId).GreaterThan(0);

        RuleFor(x => x.PlayerId).GreaterThan(0);
    }
}
