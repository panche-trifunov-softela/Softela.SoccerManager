using MediatR;

namespace SoccerManager.Application.Commands.NationalTeamMatch.DeleteNationalTeamMatch;

/// <summary>
/// Represents a request to delete an existing national team match.
/// </summary>
public sealed record DeleteNationalTeamMatchRequest : IRequest<bool>
{
    /// <summary>
    /// The identifier of the national team match to delete.
    /// </summary>
    public int Id { get; init; }
}
