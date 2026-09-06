using MediatR;

namespace SoccerManager.Application.Commands.League.CreateLeague;

/// <summary>
/// Represents a request to create a new league.
/// </summary>
public sealed record CreateLeagueRequest : IRequest<int>
{
    /// <summary>
    /// The name of the league to create.
    /// </summary>
    public required string Name { get; init; }
}
