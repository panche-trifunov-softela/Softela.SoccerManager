using SoccerManager.Application.Core.Extensions;
using SoccerManager.Application.Dtos;

namespace SoccerManager.Application.Queries.MatchPlayerAbsence.GetMatchPlayerAbsences;

/// <summary>
/// Maps match player absence domain entities to <see cref="MatchPlayerAbsenceDto"/> instances.
/// </summary>
public static class GetMatchPlayerAbsencesMapper
{
    /// <summary>
    /// Converts a match player absence entity into its DTO representation.
    /// </summary>
    /// <param name="matchPlayerAbsence">The match player absence entity to convert.</param>
    /// <returns>The corresponding <see cref="MatchPlayerAbsenceDto"/>.</returns>
    public static MatchPlayerAbsenceDto ToDto(SoccerManager.Domain.Entities.MatchPlayerAbsence matchPlayerAbsence)
    {
        return new MatchPlayerAbsenceDto
        {
            Id = matchPlayerAbsence.Id,
            MatchId = matchPlayerAbsence.MatchId,
            LeagueTeamId = matchPlayerAbsence.LeagueTeamId,
            PlayerId = matchPlayerAbsence.PlayerId,
            Reason = matchPlayerAbsence.Reason,
            ReasonLabel = matchPlayerAbsence.Reason.ToString().ToWords(),
            CreatedAt = matchPlayerAbsence.CreatedAt,
            ModifiedAt = matchPlayerAbsence.ModifiedAt,
        };
    }
}
