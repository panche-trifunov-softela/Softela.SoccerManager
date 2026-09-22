using SoccerManager.Application.Dtos;

namespace SoccerManager.Application.Queries.Standing.GetStandings;

/// <summary>
/// Maps standing domain entities to <see cref="StandingDto"/> instances.
/// </summary>
public static class GetStandingsMapper
{
    /// <summary>
    /// Converts a standing entity into its DTO representation.
    /// </summary>
    /// <param name="standing">The standing entity to convert.</param>
    /// <returns>The corresponding <see cref="StandingDto"/>.</returns>
    public static StandingDto ToDto(SoccerManager.Domain.Entities.Standing standing)
    {
        return new StandingDto
        {
            Id = standing.Id,
            CompetitionId = standing.CompetitionId,
            SeasonId = standing.SeasonId,
            DivisionId = standing.DivisionId,
            TeamId = standing.TeamId,
            Points = standing.Points,
            GoalsFor = standing.GoalsFor,
            GoalsAgainst = standing.GoalsAgainst,
            Wins = standing.Wins,
            Draws = standing.Draws,
            Losses = standing.Losses,
            CreatedAt = standing.CreatedAt,
            ModifiedAt = standing.ModifiedAt,
        };
    }
}
