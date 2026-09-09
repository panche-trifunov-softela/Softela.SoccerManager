using MediatR;

namespace SoccerManager.Application.Commands.Division.UpdateDivision;

/// <summary>
/// Represents a request to update an existing division.
/// </summary>
public sealed record UpdateDivisionRequest : IRequest<bool>
{
    /// <summary>
    /// The identifier of the division to update.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// The new name of the division.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// The new rank of the division within its league, from top to bottom.
    /// </summary>
    public int Order { get; init; }

    /// <summary>
    /// The new number of teams promoted from this division at the end of a season.
    /// </summary>
    public int TeamsPromoted { get; init; }

    /// <summary>
    /// The new number of teams relegated from this division at the end of a season.
    /// </summary>
    public int TeamsRelegated { get; init; }

    /// <summary>
    /// The new number of teams from this division that enter the promotion playoffs.
    /// </summary>
    public int TeamsInPlayoffs { get; init; }
}
