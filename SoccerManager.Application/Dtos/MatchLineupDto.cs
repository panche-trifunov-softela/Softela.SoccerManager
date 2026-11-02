namespace SoccerManager.Application.Dtos;

/// <summary>
/// Represents a club's lineup for a match: its formation and every slot of it.
/// </summary>
public sealed record MatchLineupDto
{
    /// <summary>
    /// The identifier of the match.
    /// </summary>
    public int MatchId { get; init; }

    /// <summary>
    /// The identifier of the league team (the club in its league) the lineup belongs to.
    /// </summary>
    public int LeagueTeamId { get; init; }

    /// <summary>
    /// The identifier of the lineup's formation, or <see langword="null"/> when no complete formation exists.
    /// </summary>
    public int? FormationId { get; init; }

    /// <summary>
    /// The name of the lineup's formation, or <see langword="null"/> when no complete formation exists.
    /// </summary>
    public string? FormationName { get; init; }

    /// <summary>
    /// Every slot of the formation in slot order, filled or empty. An empty lineup still lists the formation's slots, all
    /// empty; the list is empty only when no complete formation exists.
    /// </summary>
    public required IReadOnlyList<MatchLineupSlotDto> Slots { get; init; }
}
