using FluentValidation;

namespace SoccerManager.Application.Queries.FormationPosition.GetFormationPositions;

/// <summary>
/// Validates <see cref="GetFormationPositionsRequest"/> instances.
/// </summary>
public sealed class GetFormationPositionsValidator : AbstractValidator<GetFormationPositionsRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetFormationPositionsValidator"/> class.
    /// </summary>
    public GetFormationPositionsValidator()
    {
        // This query is always scoped to a formation, so a missing or zero
        // FormationId must fail fast here rather than silently returning nothing.
        RuleFor(x => x.FormationId).GreaterThan(0);
    }
}
