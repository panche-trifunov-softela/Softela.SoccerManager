using MediatR;

namespace SoccerManager.Application.Commands.LeagueTeamManager.CreateLeagueTeamManager;

/// <summary>
/// Represents a request to create a new league team manager appointment.
/// </summary>
public sealed record CreateLeagueTeamManagerRequest : IRequest<int>
{
    /// <summary>
    /// The identifier of the league roster entry (the club in its league) the manager is appointed to.
    /// </summary>
    public int LeagueTeamId { get; init; }

    /// <summary>
    /// The identifier of the manager the appointment belongs to.
    /// </summary>
    public int ManagerId { get; init; }

    /// <summary>
    /// The date the tenure started.
    /// </summary>
    public DateOnly StartDate { get; init; }

    /// <summary>
    /// The date the tenure ended, or <see langword="null"/> while it is still current.
    /// </summary>
    public DateOnly? EndDate { get; init; }

    /// <summary>
    /// Whether the appointment is the team's current manager in the league.
    /// </summary>
    public bool IsCurrent { get; init; }
}
