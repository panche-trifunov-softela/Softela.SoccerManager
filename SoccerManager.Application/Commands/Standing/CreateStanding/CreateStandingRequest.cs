using MediatR;

namespace SoccerManager.Application.Commands.Standing.CreateStanding;

/// <summary>
/// Represents a request to create a new standing.
/// </summary>
public sealed record CreateStandingRequest : IRequest<int>
{
    /// <summary>
    /// The identifier of the competition this record belongs to.
    /// </summary>
    public int CompetitionId { get; init; }

    /// <summary>
    /// The identifier of the season this record belongs to.
    /// </summary>
    public int SeasonId { get; init; }

    /// <summary>
    /// The identifier of the division this record belongs to.
    /// </summary>
    public int DivisionId { get; init; }

    /// <summary>
    /// The identifier of the team this record is for.
    /// </summary>
    public int TeamId { get; init; }

    /// <summary>
    /// The points accumulated.
    /// </summary>
    public int Points { get; init; }

    /// <summary>
    /// The goals scored.
    /// </summary>
    public int GoalsFor { get; init; }

    /// <summary>
    /// The goals conceded.
    /// </summary>
    public int GoalsAgainst { get; init; }

    /// <summary>
    /// The number of matches won.
    /// </summary>
    public int Wins { get; init; }

    /// <summary>
    /// The number of matches drawn.
    /// </summary>
    public int Draws { get; init; }

    /// <summary>
    /// The number of matches lost.
    /// </summary>
    public int Losses { get; init; }
}
