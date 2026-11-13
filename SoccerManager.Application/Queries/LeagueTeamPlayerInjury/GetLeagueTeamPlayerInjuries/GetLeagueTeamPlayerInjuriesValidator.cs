using FluentValidation;

namespace SoccerManager.Application.Queries.LeagueTeamPlayerInjury.GetLeagueTeamPlayerInjuries;

/// <summary>
/// Validates <see cref="GetLeagueTeamPlayerInjuriesRequest"/> instances.
/// </summary>
public sealed class GetLeagueTeamPlayerInjuriesValidator : AbstractValidator<GetLeagueTeamPlayerInjuriesRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetLeagueTeamPlayerInjuriesValidator"/> class.
    /// </summary>
    public GetLeagueTeamPlayerInjuriesValidator()
    {
        // This query is always scoped to a squad row, so a missing or zero
        // LeagueTeamPlayerId must fail fast here rather than silently returning nothing.
        RuleFor(x => x.LeagueTeamPlayerId).GreaterThan(0);
    }
}
