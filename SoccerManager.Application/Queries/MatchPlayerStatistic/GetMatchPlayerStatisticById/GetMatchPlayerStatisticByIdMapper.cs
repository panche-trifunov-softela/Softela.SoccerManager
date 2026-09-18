using SoccerManager.Application.Dtos;

namespace SoccerManager.Application.Queries.MatchPlayerStatistic.GetMatchPlayerStatisticById;

/// <summary>
/// Maps match player statistic domain entities to <see cref="MatchPlayerStatisticDto"/> instances.
/// </summary>
public static class GetMatchPlayerStatisticByIdMapper
{
    /// <summary>
    /// Converts a match player statistic entity into its DTO representation.
    /// </summary>
    /// <param name="matchPlayerStatistic">The match player statistic entity to convert.</param>
    /// <returns>The corresponding <see cref="MatchPlayerStatisticDto"/>.</returns>
    public static MatchPlayerStatisticDto ToDto(SoccerManager.Domain.Entities.MatchPlayerStatistic matchPlayerStatistic)
    {
        return new MatchPlayerStatisticDto
        {
            Id = matchPlayerStatistic.Id,
            PlayerId = matchPlayerStatistic.PlayerId,
            MatchId = matchPlayerStatistic.MatchId,
            Rating = matchPlayerStatistic.Rating,
            IsStarter = matchPlayerStatistic.IsStarter,
            MinutesPlayed = matchPlayerStatistic.MinutesPlayed,
            Goals = matchPlayerStatistic.Goals,
            Assists = matchPlayerStatistic.Assists,
            PenaltiesScored = matchPlayerStatistic.PenaltiesScored,
            PenaltiesMissed = matchPlayerStatistic.PenaltiesMissed,
            YellowCards = matchPlayerStatistic.YellowCards,
            RedCards = matchPlayerStatistic.RedCards,
            CreatedAt = matchPlayerStatistic.CreatedAt,
            ModifiedAt = matchPlayerStatistic.ModifiedAt,
        };
    }
}
