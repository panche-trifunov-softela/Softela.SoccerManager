using FluentValidation;

namespace SoccerManager.Application.Queries.MatchPlayerStatistic.GetMatchPlayerStatistics;

/// <summary>
/// Validates <see cref="GetMatchPlayerStatisticsRequest"/> instances.
/// </summary>
public sealed class GetMatchPlayerStatisticsValidator : AbstractValidator<GetMatchPlayerStatisticsRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetMatchPlayerStatisticsValidator"/> class.
    /// </summary>
    public GetMatchPlayerStatisticsValidator()
    {
        // This query is always scoped to a match, so a missing or zero
        // MatchId must fail fast here rather than silently returning nothing.
        RuleFor(x => x.MatchId).GreaterThan(0);
    }
}
