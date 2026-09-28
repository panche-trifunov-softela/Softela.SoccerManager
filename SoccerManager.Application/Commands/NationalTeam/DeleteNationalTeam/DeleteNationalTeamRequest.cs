using MediatR;

namespace SoccerManager.Application.Commands.NationalTeam.DeleteNationalTeam;

/// <summary>
/// Represents a request to delete an existing national team.
/// </summary>
public sealed record DeleteNationalTeamRequest : IRequest<bool>
{
    /// <summary>
    /// The identifier of the national team to delete.
    /// </summary>
    public int Id { get; init; }
}
