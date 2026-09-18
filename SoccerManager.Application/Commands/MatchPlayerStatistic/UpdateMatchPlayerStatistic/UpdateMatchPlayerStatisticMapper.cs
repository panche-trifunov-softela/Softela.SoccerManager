namespace SoccerManager.Application.Commands.MatchPlayerStatistic.UpdateMatchPlayerStatistic;

/// <summary>
/// Applies <see cref="UpdateMatchPlayerStatisticRequest"/> values onto an existing domain entity.
/// </summary>
public static class UpdateMatchPlayerStatisticMapper
{
    /// <summary>
    /// Applies the request values to the given match player statistic, preserving its creation metadata.
    /// </summary>
    /// <param name="request">The request carrying the new match player statistic values.</param>
    /// <param name="matchPlayerStatistic">The loaded match player statistic entity to mutate.</param>
    /// <param name="now">The UTC timestamp to stamp on the modified field.</param>
    /// <param name="userId">The identifier of the user performing the update.</param>
    public static void ApplyTo(UpdateMatchPlayerStatisticRequest request, SoccerManager.Domain.Entities.MatchPlayerStatistic matchPlayerStatistic, DateTime now, Guid userId)
    {
        matchPlayerStatistic.PlayerId = request.PlayerId;
        matchPlayerStatistic.MatchId = request.MatchId;
        matchPlayerStatistic.Rating = request.Rating;
        matchPlayerStatistic.IsStarter = request.IsStarter;
        matchPlayerStatistic.MinutesPlayed = request.MinutesPlayed;
        matchPlayerStatistic.Goals = request.Goals;
        matchPlayerStatistic.Assists = request.Assists;
        matchPlayerStatistic.PenaltiesScored = request.PenaltiesScored;
        matchPlayerStatistic.PenaltiesMissed = request.PenaltiesMissed;
        matchPlayerStatistic.YellowCards = request.YellowCards;
        matchPlayerStatistic.RedCards = request.RedCards;
        matchPlayerStatistic.ModifiedAt = now;
        matchPlayerStatistic.ModifiedBy = userId;
    }
}
