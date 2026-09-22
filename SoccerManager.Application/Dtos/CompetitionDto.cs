using SoccerManager.Domain.Enums;

namespace SoccerManager.Application.Dtos;

/// <summary>
/// Represents a competition for read-oriented consumers.
/// </summary>
public sealed record CompetitionDto
{
    /// <summary>
    /// The identifier of the competition.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// The identifier of the league the competition belongs to.
    /// </summary>
    public int LeagueId { get; init; }

    /// <summary>
    /// The name of the competition.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// The URL of the competition's logo image, or <see langword="null"/> when it has none.
    /// </summary>
    public string? LogoUrl { get; init; }

    /// <summary>
    /// A value indicating whether the competition is domestic.
    /// </summary>
    public bool IsDomestic { get; init; }

    /// <summary>
    /// The competition's format.
    /// </summary>
    public CompetitionFormat Format { get; init; }

    /// <summary>
    /// The maximum age allowed to compete in the competition, or <see langword="null"/> when it has no age limit.
    /// </summary>
    public int? MaxAgeAllowed { get; init; }

    /// <summary>
    /// The competition's rank within its league, where lower values indicate higher tiers, or 0 when it is not a
    /// tiered competition.
    /// </summary>
    public int Order { get; init; }

    /// <summary>
    /// The number of teams promoted from this competition at the end of a season.
    /// </summary>
    public int TeamsPromoted { get; init; }

    /// <summary>
    /// The number of teams relegated from this competition at the end of a season.
    /// </summary>
    public int TeamsRelegated { get; init; }

    /// <summary>
    /// The number of teams from this competition that enter the playoffs.
    /// </summary>
    public int TeamsInPlayoffs { get; init; }

    /// <summary>
    /// The UTC date and time the competition was created.
    /// </summary>
    public DateTime CreatedAt { get; init; }

    /// <summary>
    /// The UTC date and time the competition was last modified.
    /// </summary>
    public DateTime ModifiedAt { get; init; }
}
