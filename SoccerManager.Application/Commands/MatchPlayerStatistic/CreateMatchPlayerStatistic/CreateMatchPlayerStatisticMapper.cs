namespace SoccerManager.Application.Commands.MatchPlayerStatistic.CreateMatchPlayerStatistic;

/// <summary>
/// Maps <see cref="CreateMatchPlayerStatisticRequest"/> instances to domain entities.
/// </summary>
public static class CreateMatchPlayerStatisticMapper
{
    /// <summary>
    /// Creates a new match player statistic entity from the given request.
    /// </summary>
    /// <param name="request">The request carrying the match player statistic values.</param>
    /// <param name="now">The UTC timestamp to stamp on the created and modified fields.</param>
    /// <param name="userId">The identifier of the user creating the match player statistic.</param>
    /// <returns>A new, unsaved match player statistic entity.</returns>
    public static SoccerManager.Domain.Entities.MatchPlayerStatistic ToDomainEntity(CreateMatchPlayerStatisticRequest request, DateTime now, Guid userId)
    {
        return new SoccerManager.Domain.Entities.MatchPlayerStatistic
        {
            PlayerId = request.PlayerId,
            MatchId = request.MatchId,
            Rating = request.Rating,
            IsStarter = request.IsStarter,
            MinutesPlayed = request.MinutesPlayed,
            Goals = request.Goals,
            Assists = request.Assists,
            PenaltiesScored = request.PenaltiesScored,
            PenaltiesMissed = request.PenaltiesMissed,
            YellowCards = request.YellowCards,
            RedCards = request.RedCards,
            CreatedAt = now,
            CreatedBy = userId,
            ModifiedAt = now,
            ModifiedBy = userId,
        };
    }
}
