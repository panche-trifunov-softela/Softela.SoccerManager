using FluentValidation;

namespace SoccerManager.Application.Queries.Standing.GetStandings;

/// <summary>
/// Validates <see cref="GetStandingsRequest"/> instances.
/// </summary>
public sealed class GetStandingsValidator : AbstractValidator<GetStandingsRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetStandingsValidator"/> class.
    /// </summary>
    public GetStandingsValidator()
    {
        // This query is always scoped to one division within one season, so a
        // missing or zero SeasonId or DivisionId must fail fast rather than
        // silently returning nothing.
        RuleFor(x => x.SeasonId).GreaterThan(0);
        RuleFor(x => x.DivisionId).GreaterThan(0);
    }
}
