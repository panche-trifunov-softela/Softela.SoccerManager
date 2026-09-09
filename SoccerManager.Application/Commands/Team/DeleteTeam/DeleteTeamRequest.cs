using MediatR;

namespace SoccerManager.Application.Commands.Team.DeleteTeam;

/// <summary>
/// Represents a request to delete an existing team.
/// </summary>
public sealed record DeleteTeamRequest : IRequest<bool>
{
    /// <summary>
    /// The identifier of the team to delete.
    /// </summary>
    public int Id { get; init; }
}
