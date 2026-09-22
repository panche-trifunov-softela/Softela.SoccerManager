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
        // This query is always scoped to one season within one competition, so a
        // missing or zero CompetitionId or SeasonId must fail fast rather than
        // silently returning nothing.
        RuleFor(x => x.CompetitionId).GreaterThan(0);
        RuleFor(x => x.SeasonId).GreaterThan(0);
    }
}
