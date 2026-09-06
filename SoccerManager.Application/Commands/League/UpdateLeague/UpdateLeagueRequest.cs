using MediatR;

namespace SoccerManager.Application.Commands.League.UpdateLeague;

/// <summary>
/// Represents a request to update an existing league.
/// </summary>
public sealed record UpdateLeagueRequest : IRequest<bool>
{
    /// <summary>
    /// The identifier of the league to update.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// The new name of the league.
    /// </summary>
    public required string Name { get; init; }
}
