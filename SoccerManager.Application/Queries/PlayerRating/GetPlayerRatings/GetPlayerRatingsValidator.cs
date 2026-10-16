using FluentValidation;

namespace SoccerManager.Application.Queries.PlayerRating.GetPlayerRatings;

/// <summary>
/// Validates <see cref="GetPlayerRatingsRequest"/> instances.
/// </summary>
public sealed class GetPlayerRatingsValidator : AbstractValidator<GetPlayerRatingsRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetPlayerRatingsValidator"/> class.
    /// </summary>
    public GetPlayerRatingsValidator()
    {
        // This query is always scoped to a player, so a missing or zero
        // PlayerId must fail fast here rather than silently returning nothing.
        RuleFor(x => x.PlayerId).GreaterThan(0);
    }
}
