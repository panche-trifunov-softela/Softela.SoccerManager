using MediatR;
using SoccerManager.Application.Dtos;
using SoccerManager.Domain.Enums;

namespace SoccerManager.Application.Commands.Match.SaveMatchLineupSubstitution;

/// <summary>
/// Represents a request to change a club's substitution lineup for a match at a minute and situation: its formation, one slot,
/// or both. The first change creates the substitution from the main lineup.
/// </summary>
public sealed record SaveMatchLineupSubstitutionRequest : IRequest<IReadOnlyList<MatchSubstitutionDto>>
{
    /// <summary>
    /// The identifier of the match the substitution is for. The route supplies it and overwrites a value in the body.
    /// </summary>
    public int MatchId { get; init; }

    /// <summary>
    /// The identifier of the league team (the club in its league) whose substitution lineup to change.
    /// </summary>
    public int LeagueTeamId { get; init; }

    /// <summary>
    /// The minute of the substitution to change: 45, 60 or 75.
    /// </summary>
    public int MinuteAt { get; init; }

    /// <summary>
    /// The situation of the substitution to change.
    /// </summary>
    public Situation Situation { get; init; }

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
    /// The identifier of the player to put in the slot. A player already in another slot of the substitution's lineup is moved:
    /// the player who was in this slot takes the other slot (a swap), or the other slot is left empty when this one had no
    /// player (a move). A player not in the lineup replaces the one in the slot, who leaves the lineup.
    /// </summary>
    public int? PlayerId { get; init; }
}
