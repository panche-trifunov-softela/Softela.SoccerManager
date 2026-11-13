using FluentValidation;

namespace SoccerManager.Application.Queries.LeagueTeam.GetLeaguePlayerProfile;

/// <summary>
/// Validates <see cref="GetLeaguePlayerProfileRequest"/> instances.
/// </summary>
public sealed class GetLeaguePlayerProfileValidator : AbstractValidator<GetLeaguePlayerProfileRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetLeaguePlayerProfileValidator"/> class.
    /// </summary>
    public GetLeaguePlayerProfileValidator()
    {
        // A missing, zero or negative id fails fast here, as a 400, before the access check and the lookup, where it would be
        // a 403 or a 404.
        RuleFor(x => x.LeagueTeamId).GreaterThan(0);

        RuleFor(x => x.PlayerId).GreaterThan(0);
    }
}
