using MediatR;

namespace SoccerManager.Application.Commands.LeagueTeamManager.DeleteLeagueTeamManager;

/// <summary>
/// Represents a request to delete an existing league team manager appointment.
/// </summary>
public sealed record DeleteLeagueTeamManagerRequest : IRequest<bool>
{
    /// <summary>
    /// The identifier of the league team manager appointment to delete.
    /// </summary>
    public int Id { get; init; }
}
