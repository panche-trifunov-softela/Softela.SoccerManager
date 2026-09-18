using FluentValidation;

namespace SoccerManager.Application.Queries.MatchTeamStatistic.GetMatchTeamStatistics;

/// <summary>
/// Validates <see cref="GetMatchTeamStatisticsRequest"/> instances.
/// </summary>
public sealed class GetMatchTeamStatisticsValidator : AbstractValidator<GetMatchTeamStatisticsRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetMatchTeamStatisticsValidator"/> class.
    /// </summary>
    public GetMatchTeamStatisticsValidator()
    {
        // This query is always scoped to a match, so a missing or zero
        // MatchId must fail fast here rather than silently returning nothing.
        RuleFor(x => x.MatchId).GreaterThan(0);
    }
}
