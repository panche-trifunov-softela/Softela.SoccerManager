using MediatR;
using SoccerManager.Application.Dtos;

namespace SoccerManager.Application.Commands.Match.SaveMatchLineup;

/// <summary>
/// Represents a request to change a club's lineup for a match: its formation, one slot, or both.
/// </summary>
public sealed record SaveMatchLineupRequest : IRequest<MatchLineupDto>
{
    /// <summary>
    /// The identifier of the match the lineup is for. The route supplies it and overwrites a value in the body.
    /// </summary>
    public int MatchId { get; init; }

    /// <summary>
    /// The identifier of the league team (the club in its league) whose lineup to change.
    /// </summary>
    public int LeagueTeamId { get; init; }

    /// <summary>
    /// The identifier of the formation to start the lineup in or switch it to, or <see langword="null"/> to keep the current one.
    /// </summary>
    public int? FormationId { get; init; }

    /// <summary>
    /// The number of the slot to change, from 1 to 19 (1 to 11 are the starters and 12 to 19 the reserves), numbered by the
    /// lineup's formation after any switch, or <see langword="null"/> to change no slot.
    /// </summary>
    public int? SlotNumber { get; init; }

    /// <summary>
    /// The identifier of the player to put in the slot, or <see langword="null"/>, or omitted, to empty the slot. A player
    /// already in another slot of the lineup is moved: the player who was in this slot takes the other slot (a swap), or the
    /// other slot is left empty when this one had no player (a move). A player not in the lineup replaces the one in the slot,
    /// who leaves the lineup.
    /// </summary>
    public int? PlayerId { get; init; }
}
