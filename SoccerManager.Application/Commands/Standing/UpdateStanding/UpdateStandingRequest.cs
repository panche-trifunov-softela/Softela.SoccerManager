using MediatR;

namespace SoccerManager.Application.Commands.Standing.UpdateStanding;

/// <summary>
/// Represents a request to update an existing standing.
/// </summary>
public sealed record UpdateStandingRequest : IRequest<bool>
{
    /// <summary>
    /// The identifier of the standing to update.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// The identifier of the competition this record belongs to.
    /// </summary>
    public int CompetitionId { get; init; }

    /// <summary>
    /// The identifier of the season this record belongs to.
    /// </summary>
    public int SeasonId { get; init; }

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
