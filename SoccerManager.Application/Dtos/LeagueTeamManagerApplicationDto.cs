using SoccerManager.Domain.Enums;

namespace SoccerManager.Application.Dtos;

/// <summary>
/// Represents a league team manager application for read-oriented consumers.
/// </summary>
public sealed record LeagueTeamManagerApplicationDto
{
    /// <summary>
    /// The identifier of the application.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// The identifier of the league the application is for.
    /// </summary>
    public int LeagueId { get; init; }

    /// <summary>
    /// The identifier of the team the manager applied to manage.
    /// </summary>
    public int TeamId { get; init; }

    /// <summary>
    /// The identifier of the manager who applied.
    /// </summary>
    public int ManagerId { get; init; }

    /// <summary>
    /// Where the application stands.
    /// </summary>
    public ApplicationStatus Status { get; init; }

    /// <summary>
    /// The UTC date and time the application was answered, or <see langword="null"/> while it is still pending.
    /// </summary>
    public DateTime? ResponseDate { get; init; }

    /// <summary>
    /// The UTC date and time the application was created.
    /// </summary>
    public DateTime CreatedAt { get; init; }

    /// <summary>
    /// The UTC date and time the application was last modified.
    /// </summary>
    public DateTime ModifiedAt { get; init; }
}
