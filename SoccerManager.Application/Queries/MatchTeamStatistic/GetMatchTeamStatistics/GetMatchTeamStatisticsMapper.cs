using SoccerManager.Application.Dtos;

namespace SoccerManager.Application.Queries.MatchTeamStatistic.GetMatchTeamStatistics;

/// <summary>
/// Maps match team statistic domain entities to <see cref="MatchTeamStatisticDto"/> instances.
/// </summary>
public static class GetMatchTeamStatisticsMapper
{
    /// <summary>
    /// Converts a match team statistic entity into its DTO representation.
    /// </summary>
    /// <param name="matchTeamStatistic">The match team statistic entity to convert.</param>
    /// <returns>The corresponding <see cref="MatchTeamStatisticDto"/>.</returns>
    public static MatchTeamStatisticDto ToDto(SoccerManager.Domain.Entities.MatchTeamStatistic matchTeamStatistic)
    {
        return new MatchTeamStatisticDto
        {
            Id = matchTeamStatistic.Id,
            TeamId = matchTeamStatistic.TeamId,
            MatchId = matchTeamStatistic.MatchId,
            IsHomeTeam = matchTeamStatistic.IsHomeTeam,
            ShotsTotal = matchTeamStatistic.ShotsTotal,
            ShotsOnTarget = matchTeamStatistic.ShotsOnTarget,
            Possession = matchTeamStatistic.Possession,
            CreatedAt = matchTeamStatistic.CreatedAt,
            ModifiedAt = matchTeamStatistic.ModifiedAt,
        };
    }
}
