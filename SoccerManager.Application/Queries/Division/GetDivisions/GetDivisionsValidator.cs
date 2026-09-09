using FluentValidation;

namespace SoccerManager.Application.Queries.Division.GetDivisions;

/// <summary>
/// Validates <see cref="GetDivisionsRequest"/> instances.
/// </summary>
public sealed class GetDivisionsValidator : AbstractValidator<GetDivisionsRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetDivisionsValidator"/> class.
    /// </summary>
    public GetDivisionsValidator()
    {
        // Unlike GetLeagues, this query is always scoped to a league, so a missing
        // or zero LeagueId must fail fast here rather than silently returning nothing.
        RuleFor(x => x.LeagueId).GreaterThan(0);
    }
}
