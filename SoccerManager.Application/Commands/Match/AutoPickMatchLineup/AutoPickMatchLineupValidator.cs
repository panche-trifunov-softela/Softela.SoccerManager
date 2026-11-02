using FluentValidation;

namespace SoccerManager.Application.Commands.Match.AutoPickMatchLineup;

/// <summary>
/// Validates <see cref="AutoPickMatchLineupRequest"/> instances.
/// </summary>
public sealed class AutoPickMatchLineupValidator : AbstractValidator<AutoPickMatchLineupRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AutoPickMatchLineupValidator"/> class.
    /// </summary>
    public AutoPickMatchLineupValidator()
    {
        RuleFor(x => x.MatchId).GreaterThan(0);
        RuleFor(x => x.LeagueTeamId).GreaterThan(0);
        RuleFor(x => x.FormationId).GreaterThan(0).When(x => x.FormationId.HasValue);
    }
}
