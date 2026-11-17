namespace SoccerManager.Application.Dtos.LeagueSearch;

/// <summary>
/// Represents the best matching players of a league search, with the number of players matching in all.
/// </summary>
public sealed record LeagueSearchPlayerGroupDto
{
    /// <summary>
    /// The best matching players: names where the search starts a word first, then the best rated, then by name; empty when
    /// none match.
    /// </summary>
    public required List<LeagueSearchPlayerItemDto> Items { get; init; }

    /// <summary>
    /// The number of players matching the search, however many <see cref="Items"/> holds.
    /// </summary>
    public int TotalCount { get; init; }
}
