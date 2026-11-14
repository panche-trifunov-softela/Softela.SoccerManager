using FluentValidation;
using FluentValidation.Results;
using SoccerManager.Application.Repositories;
using SoccerManager.Domain.Entities;

namespace SoccerManager.Application.Services;

/// <summary>
/// Holds the rules an absence's create and update share, so both are checked by one definition: the club plays the match,
/// the player is in the club's squad, the player has no other absence in the match, and the player did not play in it.
/// </summary>
public class MatchPlayerAbsenceService : IMatchPlayerAbsenceService
{
    private readonly IMatchLineupService _matchLineupService;
    private readonly ILeagueTeamPlayerRepository _leagueTeamPlayerRepository;
    private readonly IMatchPlayerAbsenceRepository _matchPlayerAbsenceRepository;
    private readonly IMatchPlayerStatisticRepository _matchPlayerStatisticRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="MatchPlayerAbsenceService"/> class.
    /// </summary>
    /// <param name="matchLineupService">The service used to check that the club plays the match.</param>
    /// <param name="leagueTeamPlayerRepository">The repository used to load the club's squad.</param>
    /// <param name="matchPlayerAbsenceRepository">The repository used to load the match's absences.</param>
    /// <param name="matchPlayerStatisticRepository">The repository used to load the match's player statistics.</param>
    public MatchPlayerAbsenceService(IMatchLineupService matchLineupService, ILeagueTeamPlayerRepository leagueTeamPlayerRepository, IMatchPlayerAbsenceRepository matchPlayerAbsenceRepository, IMatchPlayerStatisticRepository matchPlayerStatisticRepository)
    {
        _matchLineupService = matchLineupService;
        _leagueTeamPlayerRepository = leagueTeamPlayerRepository;
        _matchPlayerAbsenceRepository = matchPlayerAbsenceRepository;
        _matchPlayerStatisticRepository = matchPlayerStatisticRepository;
    }

    /// <summary>
    /// Ensures a player's absence from a match is allowed. It checks, in this order, that the club plays the match, that the
    /// player is in the club's squad, that the player has no other absence in the match, and that the player played no
    /// minutes in it.
    /// </summary>
    /// <param name="matchId">The identifier of the match the player misses.</param>
    /// <param name="leagueTeamId">The identifier of the league team (the club in its league) the player is missing from.</param>
    /// <param name="playerId">The identifier of the player who is absent.</param>
    /// <param name="ownId">The identifier of the absence being updated, so it does not count as a second absence; <see langword="null"/> when an absence is created.</param>
    /// <exception cref="KeyNotFoundException">Thrown when the match does not exist.</exception>
    /// <exception cref="ValidationException">Thrown when the club does not play in the match, the player is not in the club's squad, the player already has an absence in the match, or the player played in it.</exception>
    public async Task EnsureAbsenceIsAllowedAsync(int matchId, int leagueTeamId, int playerId, int? ownId)
    {
        await _matchLineupService.EnsureClubPlaysMatchAsync(matchId, leagueTeamId);

        var squad = await _leagueTeamPlayerRepository.GetByLeagueTeamIdAsync(leagueTeamId);

        if (!squad.Any(squadRow => squadRow.PlayerId == playerId))
        {
            throw new ValidationException(new[]
            {
                new ValidationFailure(nameof(MatchPlayerAbsence.PlayerId), "The player is not in the club's squad."),
            });
        }

        // Every club of the match counts, as the unique key does; the absence being updated is not a second one.
        var absences = await _matchPlayerAbsenceRepository.GetByMatchIdAsync(matchId);

        if (absences.Any(absence => absence.PlayerId == playerId && absence.Id != ownId))
        {
            throw new ValidationException(new[]
            {
                new ValidationFailure(nameof(MatchPlayerAbsence.PlayerId), "The player already has an absence in this match."),
            });
        }

        var statistics = await _matchPlayerStatisticRepository.GetByMatchIdAsync(matchId);

        if (statistics.Any(statistic => statistic.PlayerId == playerId && statistic.MinutesPlayed > 0))
        {
            throw new ValidationException(new[]
            {
                new ValidationFailure(nameof(MatchPlayerAbsence.PlayerId), "The player played in this match."),
            });
        }
    }
}
