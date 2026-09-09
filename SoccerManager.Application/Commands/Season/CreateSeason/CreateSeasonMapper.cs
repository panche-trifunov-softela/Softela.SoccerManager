namespace SoccerManager.Application.Commands.Season.CreateSeason;

/// <summary>
/// Maps <see cref="CreateSeasonRequest"/> instances to domain entities.
/// </summary>
public static class CreateSeasonMapper
{
    /// <summary>
    /// Creates a new season entity from the given request.
    /// </summary>
    /// <param name="request">The request carrying the league identifier and season number.</param>
    /// <param name="now">The UTC timestamp to stamp on the created and modified fields.</param>
    /// <param name="userId">The identifier of the user creating the season.</param>
    /// <returns>A new, unsaved season entity.</returns>
    public static SoccerManager.Domain.Entities.Season ToDomainEntity(CreateSeasonRequest request, DateTime now, Guid userId)
    {
        return new SoccerManager.Domain.Entities.Season
        {
            LeagueId = request.LeagueId,
            SeasonNumber = request.SeasonNumber,
            CreatedAt = now,
            CreatedBy = userId,
            ModifiedAt = now,
            ModifiedBy = userId,
        };
    }
}
