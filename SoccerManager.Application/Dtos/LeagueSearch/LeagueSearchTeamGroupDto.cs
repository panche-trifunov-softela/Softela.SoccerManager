namespace SoccerManager.Application.Dtos.LeagueSearch;

/// <summary>
/// Represents the best matching clubs of a league search, with the number of clubs matching in all.
/// </summary>
public sealed record LeagueSearchTeamGroupDto
{
    /// <summary>
    /// The best matching clubs: names where the search starts a word first, then by name; empty when none match.
    /// </summary>
    public required List<LeagueSearchTeamItemDto> Items { get; init; }

    /// <summary>
    /// The number of clubs matching the search, however many <see cref="Items"/> holds.
    /// </summary>
    public int TotalCount { get; init; }
}
