using SoccerManager.Application.Dtos;

namespace SoccerManager.Application.Queries.NationalTeamMatch.GetNationalTeamMatches;

/// <summary>
/// Maps national team match domain entities to <see cref="NationalTeamMatchDto"/> instances.
/// </summary>
public static class GetNationalTeamMatchesMapper
{
    /// <summary>
    /// Converts a national team match entity into its DTO representation.
    /// </summary>
    /// <param name="nationalTeamMatch">The national team match entity to convert.</param>
    /// <returns>The corresponding <see cref="NationalTeamMatchDto"/>.</returns>
    public static NationalTeamMatchDto ToDto(SoccerManager.Domain.Entities.NationalTeamMatch nationalTeamMatch)
    {
        return new NationalTeamMatchDto
        {
            Id = nationalTeamMatch.Id,
            SeasonId = nationalTeamMatch.SeasonId,
            HomeNationalTeamId = nationalTeamMatch.HomeNationalTeamId,
            AwayNationalTeamId = nationalTeamMatch.AwayNationalTeamId,
            StartDateTime = nationalTeamMatch.StartDateTime,
            Stage = nationalTeamMatch.Stage,
            GroupNumber = nationalTeamMatch.GroupNumber,
            RoundNumber = nationalTeamMatch.RoundNumber,
            Leg = nationalTeamMatch.Leg,
            IsStarted = nationalTeamMatch.IsStarted,
            IsFinished = nationalTeamMatch.IsFinished,
            CreatedAt = nationalTeamMatch.CreatedAt,
            ModifiedAt = nationalTeamMatch.ModifiedAt,
        };
    }
}
