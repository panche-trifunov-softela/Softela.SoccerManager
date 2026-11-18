namespace SoccerManager.Application.Commands.NationalTeamMatch.CreateNationalTeamMatch;

/// <summary>
/// Maps <see cref="CreateNationalTeamMatchRequest"/> instances to domain entities.
/// </summary>
public static class CreateNationalTeamMatchMapper
{
    /// <summary>
    /// Creates a new national team match entity from the given request.
    /// </summary>
    /// <param name="request">The request carrying the national team match values.</param>
    /// <param name="now">The UTC timestamp to stamp on the created and modified fields.</param>
    /// <param name="userId">The identifier of the user creating the national team match.</param>
    /// <returns>A new, unsaved national team match entity.</returns>
    public static SoccerManager.Domain.Entities.NationalTeamMatch ToDomainEntity(CreateNationalTeamMatchRequest request, DateTime now, Guid userId)
    {
        return new SoccerManager.Domain.Entities.NationalTeamMatch
        {
            SeasonId = request.SeasonId,
            HomeNationalTeamId = request.HomeNationalTeamId,
            AwayNationalTeamId = request.AwayNationalTeamId,
            StartDateTime = request.StartDateTime,
            Stage = request.Stage,
            GroupNumber = request.GroupNumber,
            RoundNumber = request.RoundNumber,
            Leg = request.Leg,
            IsStarted = request.IsStarted,
            CreatedAt = now,
            CreatedBy = userId,
            ModifiedAt = now,
            ModifiedBy = userId,
        };
    }
}
