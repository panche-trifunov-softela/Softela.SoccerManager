using SoccerManager.Application.Dtos;

namespace SoccerManager.Application.Queries.Competition.GetCompetitionById;

/// <summary>
/// Maps competition domain entities to <see cref="CompetitionDto"/> instances.
/// </summary>
public static class GetCompetitionByIdMapper
{
    /// <summary>
    /// Converts a competition entity into its DTO representation.
    /// </summary>
    /// <param name="competition">The competition entity to convert.</param>
    /// <returns>The corresponding <see cref="CompetitionDto"/>.</returns>
    public static CompetitionDto ToDto(SoccerManager.Domain.Entities.Competition competition)
    {
        return new CompetitionDto
        {
            Id = competition.Id,
            LeagueId = competition.LeagueId,
            Name = competition.Name,
            LogoUrl = competition.LogoUrl,
            IsDomestic = competition.IsDomestic,
            Format = competition.Format,
            MaxAgeAllowed = competition.MaxAgeAllowed,
            Order = competition.Order,
            TeamsPromoted = competition.TeamsPromoted,
            TeamsRelegated = competition.TeamsRelegated,
            TeamsInPlayoffs = competition.TeamsInPlayoffs,
            CreatedAt = competition.CreatedAt,
            ModifiedAt = competition.ModifiedAt,
        };
    }
}
