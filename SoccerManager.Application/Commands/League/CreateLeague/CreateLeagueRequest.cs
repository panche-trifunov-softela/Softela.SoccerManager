using MediatR;
using SoccerManager.Domain.Enums;

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

    /// <summary>
    /// The identifiers of the teams that play in the league's divisions; every other team joins the league as a guest.
    /// </summary>
    public required IReadOnlyList<int> DivisionTeamIds { get; init; }

    /// <summary>
    /// Whether the division clubs start with the real players of their teams or with empty squads; guests always get
    /// their real players.
    /// </summary>
    public required DivisionSquads DivisionSquads { get; init; }
}
