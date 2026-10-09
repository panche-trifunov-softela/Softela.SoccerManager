using MediatR;

namespace SoccerManager.Application.Commands.LeagueTeamManagerApplication.DeleteLeagueTeamManagerApplication;

/// <summary>
/// Represents a request to delete an existing league team manager application.
/// </summary>
public sealed record DeleteLeagueTeamManagerApplicationRequest : IRequest<bool>
{
    /// <summary>
    /// The identifier of the league team manager application to delete.
    /// </summary>
    public int Id { get; init; }
}
