namespace SoccerManager.Application.Commands.MatchTeamStatistic.CreateMatchTeamStatistic;

/// <summary>
/// Maps <see cref="CreateMatchTeamStatisticRequest"/> instances to domain entities.
/// </summary>
public static class CreateMatchTeamStatisticMapper
{
    /// <summary>
    /// Creates a new match team statistic entity from the given request.
    /// </summary>
    /// <param name="request">The request carrying the match team statistic values.</param>
    /// <param name="now">The UTC timestamp to stamp on the created and modified fields.</param>
    /// <param name="userId">The identifier of the user creating the match team statistic.</param>
    /// <returns>A new, unsaved match team statistic entity.</returns>
    public static SoccerManager.Domain.Entities.MatchTeamStatistic ToDomainEntity(CreateMatchTeamStatisticRequest request, DateTime now, Guid userId)
    {
        return new SoccerManager.Domain.Entities.MatchTeamStatistic
        {
            TeamId = request.TeamId,
            MatchId = request.MatchId,
            IsHomeTeam = request.IsHomeTeam,
            ShotsTotal = request.ShotsTotal,
            ShotsOnTarget = request.ShotsOnTarget,
            Possession = request.Possession,
            CreatedAt = now,
            CreatedBy = userId,
            ModifiedAt = now,
            ModifiedBy = userId,
        };
    }
}
