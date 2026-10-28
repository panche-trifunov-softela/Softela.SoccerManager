using FluentValidation;
using FluentValidation.Results;
using SoccerManager.Application.Repositories;
using SoccerManager.Domain.Entities;

namespace SoccerManager.Application.Services;

/// <summary>
/// Enforces that a lineup never includes a suspended player or the same player twice.
/// </summary>
public class MatchFormationPlayerPositionRules : IMatchFormationPlayerPositionRules
{
    private readonly IMatchFormationPlayerPositionRepository _matchFormationPlayerPositionRepository;
    private readonly IPlayerPositionRepository _playerPositionRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="MatchFormationPlayerPositionRules"/> class.
    /// </summary>
    /// <param name="matchFormationPlayerPositionRepository">The repository used to load the match's existing lineup slots.</param>
    /// <param name="playerPositionRepository">The repository used to reach the player behind each slot's position rating.</param>
    public MatchFormationPlayerPositionRules(IMatchFormationPlayerPositionRepository matchFormationPlayerPositionRepository, IPlayerPositionRepository playerPositionRepository)
    {
        _matchFormationPlayerPositionRepository = matchFormationPlayerPositionRepository;
        _playerPositionRepository = playerPositionRepository;
    }

    /// <summary>
    /// Ensures the player filling the slot is not suspended and has no other slot in the club's lineup for the match.
    /// </summary>
    /// <param name="matchId">The identifier of the match.</param>
    /// <param name="leagueTeamId">The identifier of the league roster entry (the club in its league) the player is lining up for.</param>
    /// <param name="playerPositionId">The identifier of the player position rating filling the slot.</param>
    /// <param name="isSuspended">Whether the player is suspended for the match, as resolved for the slot's snapshot.</param>
    /// <param name="excludedSlotId">The identifier of the slot being updated, which does not count as another slot, or <see langword="null"/> for a new slot.</param>
    /// <exception cref="KeyNotFoundException">Thrown when a player position involved does not exist.</exception>
    /// <exception cref="ValidationException">Thrown when the player is suspended or already has a slot in the lineup.</exception>
    public async Task EnsureValidAsync(int matchId, int leagueTeamId, int playerPositionId, bool isSuspended, int? excludedSlotId)
    {
        if (isSuspended)
        {
            throw BuildFailure("The player is suspended.");
        }

        var playerPosition = await _playerPositionRepository.GetByIdAsync(playerPositionId)
            ?? throw new KeyNotFoundException($"Player position {playerPositionId} not found.");

        var slots = await _matchFormationPlayerPositionRepository.GetByMatchIdAsync(matchId);

        var otherSlotsInLineup = slots.Where(slot => slot.LeagueTeamId == leagueTeamId && slot.Id != excludedSlotId);

        // A player has several position ratings, so the slots are compared by player, not by position rating.
        foreach (var slot in otherSlotsInLineup)
        {
            var slotPlayerPosition = await _playerPositionRepository.GetByIdAsync(slot.PlayerPositionId)
                ?? throw new KeyNotFoundException($"Player position {slot.PlayerPositionId} not found.");

            if (slotPlayerPosition.PlayerId == playerPosition.PlayerId)
            {
                throw BuildFailure("The player already has a slot in this lineup.");
            }
        }
    }

    /// <summary>
    /// Builds the validation exception raised for a rule broken by the slot's player position.
    /// </summary>
    /// <param name="message">The failure message.</param>
    /// <returns>The exception to throw.</returns>
    private static ValidationException BuildFailure(string message)
    {
        return new ValidationException(new[]
        {
            new ValidationFailure(nameof(MatchFormationPlayerPosition.PlayerPositionId), message),
        });
    }
}
