using MediatR;

namespace SoccerManager.Application.Commands.League.DeleteLeague;

/// <summary>
/// Represents a request to delete an existing league.
/// </summary>
public sealed record DeleteLeagueRequest : IRequest<bool>
{
    /// <summary>
    /// The identifier of the league to delete.
    /// </summary>
    public int Id { get; init; }
}
