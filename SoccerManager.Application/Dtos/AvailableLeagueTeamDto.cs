using SoccerManager.Domain.Enums;

namespace SoccerManager.Application.Dtos;

/// <summary>
/// Represents a club on one league's roster, as the available-clubs list shows it, with whether it
/// currently has a manager.
/// </summary>
public sealed record AvailableLeagueTeamDto
{
    /// <summary>
    /// The identifier of the club's entry on the league's roster: the league team id that applying sends.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// The name of the team.
    /// </summary>
    public required string TeamName { get; init; }

    /// <summary>
    /// The URL of the team's logo image, or <see langword="null"/> when it has none.
    /// </summary>
    public string? TeamLogoUrl { get; init; }

    /// <summary>
    /// The financial state of the team.
    /// </summary>
    public FinancialState TeamFinancialState { get; init; }

    /// <summary>
    /// Whether the club has a current manager in the league; a managed club cannot be applied for.
    /// </summary>
    public bool IsManaged { get; init; }
}
