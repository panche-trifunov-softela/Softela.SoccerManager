namespace SoccerManager.Application.Commands.Competition.CreateCompetition;

/// <summary>
/// Maps <see cref="CreateCompetitionRequest"/> instances to domain entities.
/// </summary>
public static class CreateCompetitionMapper
{
    /// <summary>
    /// Creates a new competition entity from the given request.
    /// </summary>
    /// <param name="request">The request carrying the competition values.</param>
    /// <param name="now">The UTC timestamp to stamp on the created and modified fields.</param>
    /// <param name="userId">The identifier of the user creating the competition.</param>
    /// <returns>A new, unsaved competition entity.</returns>
    public static SoccerManager.Domain.Entities.Competition ToDomainEntity(CreateCompetitionRequest request, DateTime now, Guid userId)
    {
        return new SoccerManager.Domain.Entities.Competition
        {
            LeagueId = request.LeagueId,
            Name = request.Name,
            LogoUrl = request.LogoUrl,
            IsDomestic = request.IsDomestic,
            Format = request.Format,
            MaxAgeAllowed = request.MaxAgeAllowed,
            Order = request.Order,
            TeamsPromoted = request.TeamsPromoted,
            TeamsRelegated = request.TeamsRelegated,
            TeamsInPlayoffs = request.TeamsInPlayoffs,
            CreatedAt = now,
            CreatedBy = userId,
            ModifiedAt = now,
            ModifiedBy = userId,
        };
    }
}
