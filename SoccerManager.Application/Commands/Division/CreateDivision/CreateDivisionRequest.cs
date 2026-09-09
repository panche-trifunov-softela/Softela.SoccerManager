using MediatR;

namespace SoccerManager.Application.Commands.Division.CreateDivision;

/// <summary>
/// Represents a request to create a new division.
/// </summary>
public sealed record CreateDivisionRequest : IRequest<int>
{
    /// <summary>
    /// The identifier of the league the division belongs to.
    /// </summary>
    public int LeagueId { get; init; }

    /// <summary>
    /// The name of the division to create.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// The rank of the division within its league, from top to bottom.
    /// </summary>
    public int Order { get; init; }

    /// <summary>
    /// The number of teams promoted from this division at the end of a season.
    /// </summary>
    public int TeamsPromoted { get; init; }

    /// <summary>
    /// The number of teams relegated from this division at the end of a season.
    /// </summary>
    public int TeamsRelegated { get; init; }

    /// <summary>
    /// The number of teams from this division that enter the promotion playoffs.
    /// </summary>
    public int TeamsInPlayoffs { get; init; }
}
