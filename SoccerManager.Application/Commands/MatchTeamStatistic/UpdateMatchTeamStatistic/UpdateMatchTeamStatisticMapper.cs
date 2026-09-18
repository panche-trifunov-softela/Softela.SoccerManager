namespace SoccerManager.Application.Commands.MatchTeamStatistic.UpdateMatchTeamStatistic;

/// <summary>
/// Applies <see cref="UpdateMatchTeamStatisticRequest"/> values onto an existing domain entity.
/// </summary>
public static class UpdateMatchTeamStatisticMapper
{
    /// <summary>
    /// Applies the request values to the given match team statistic, preserving its creation metadata.
    /// </summary>
    /// <param name="request">The request carrying the new match team statistic values.</param>
    /// <param name="matchTeamStatistic">The loaded match team statistic entity to mutate.</param>
    /// <param name="now">The UTC timestamp to stamp on the modified field.</param>
    /// <param name="userId">The identifier of the user performing the update.</param>
    public static void ApplyTo(UpdateMatchTeamStatisticRequest request, SoccerManager.Domain.Entities.MatchTeamStatistic matchTeamStatistic, DateTime now, Guid userId)
    {
        matchTeamStatistic.TeamId = request.TeamId;
        matchTeamStatistic.MatchId = request.MatchId;
        matchTeamStatistic.IsHomeTeam = request.IsHomeTeam;
        matchTeamStatistic.ShotsTotal = request.ShotsTotal;
        matchTeamStatistic.ShotsOnTarget = request.ShotsOnTarget;
        matchTeamStatistic.Possession = request.Possession;
        matchTeamStatistic.ModifiedAt = now;
        matchTeamStatistic.ModifiedBy = userId;
    }
}
