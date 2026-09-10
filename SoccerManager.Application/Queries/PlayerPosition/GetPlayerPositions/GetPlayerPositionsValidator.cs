using FluentValidation;

namespace SoccerManager.Application.Queries.PlayerPosition.GetPlayerPositions;

/// <summary>
/// Validates <see cref="GetPlayerPositionsRequest"/> instances.
/// </summary>
public sealed class GetPlayerPositionsValidator : AbstractValidator<GetPlayerPositionsRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetPlayerPositionsValidator"/> class.
    /// </summary>
    public GetPlayerPositionsValidator()
    {
        // This query is always scoped to a player, so a missing or zero
        // PlayerId must fail fast here rather than silently returning nothing.
        RuleFor(x => x.PlayerId).GreaterThan(0);
    }
}
