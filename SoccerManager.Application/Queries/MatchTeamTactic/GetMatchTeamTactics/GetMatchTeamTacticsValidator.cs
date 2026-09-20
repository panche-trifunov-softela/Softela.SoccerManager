using FluentValidation;

namespace SoccerManager.Application.Queries.MatchTeamTactic.GetMatchTeamTactics;

/// <summary>
/// Validates <see cref="GetMatchTeamTacticsRequest"/> instances.
/// </summary>
public sealed class GetMatchTeamTacticsValidator : AbstractValidator<GetMatchTeamTacticsRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetMatchTeamTacticsValidator"/> class.
    /// </summary>
    public GetMatchTeamTacticsValidator()
    {
        // This query is always scoped to a match, so a missing or zero
        // MatchId must fail fast here rather than silently returning nothing.
        RuleFor(x => x.MatchId).GreaterThan(0);
    }
}
