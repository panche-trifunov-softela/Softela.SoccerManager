using FluentValidation;

namespace SoccerManager.Application.Queries.MatchFormationPlayerPosition.GetMatchFormationPlayerPositions;

/// <summary>
/// Validates <see cref="GetMatchFormationPlayerPositionsRequest"/> instances.
/// </summary>
public sealed class GetMatchFormationPlayerPositionsValidator : AbstractValidator<GetMatchFormationPlayerPositionsRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetMatchFormationPlayerPositionsValidator"/> class.
    /// </summary>
    public GetMatchFormationPlayerPositionsValidator()
    {
        // This query is always scoped to a match, so a missing or zero
        // MatchId must fail fast here rather than silently returning nothing.
        RuleFor(x => x.MatchId).GreaterThan(0);
    }
}
