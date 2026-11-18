using FluentValidation;

namespace SoccerManager.Application.Queries.NationalTeamMatch.GetNationalTeamMatches;

/// <summary>
/// Validates <see cref="GetNationalTeamMatchesRequest"/> instances.
/// </summary>
public sealed class GetNationalTeamMatchesValidator : AbstractValidator<GetNationalTeamMatchesRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetNationalTeamMatchesValidator"/> class.
    /// </summary>
    public GetNationalTeamMatchesValidator()
    {
        // This query is always scoped to a season, so a missing or zero
        // SeasonId must fail fast here rather than silently returning nothing.
        RuleFor(x => x.SeasonId).GreaterThan(0);
    }
}
