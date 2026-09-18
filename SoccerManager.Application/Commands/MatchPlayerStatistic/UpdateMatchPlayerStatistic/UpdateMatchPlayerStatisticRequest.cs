using MediatR;

namespace SoccerManager.Application.Commands.MatchPlayerStatistic.UpdateMatchPlayerStatistic;

/// <summary>
/// Represents a request to update an existing match player statistic.
/// </summary>
public sealed record UpdateMatchPlayerStatisticRequest : IRequest<bool>
{
    /// <summary>
    /// The identifier of the match player statistic to update.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// The identifier of the player the statistics belong to.
    /// </summary>
    public int PlayerId { get; init; }

    /// <summary>
    /// The identifier of the match the statistics belong to.
    /// </summary>
    public int MatchId { get; init; }

    /// <summary>
    /// The player's rating for the match, from 5.0 to 10.0 inclusive.
    /// </summary>
    public decimal Rating { get; init; }

    /// <summary>
    /// Whether the player started the match.
    /// </summary>
    public bool IsStarter { get; init; }

    /// <summary>
    /// The number of minutes the player played.
    /// </summary>
    public int MinutesPlayed { get; init; }

    /// <summary>
    /// The total number of goals scored, including penalties scored.
    /// </summary>
    public int Goals { get; init; }

    /// <summary>
    /// The number of assists made.
    /// </summary>
    public int Assists { get; init; }

    /// <summary>
    /// The number of penalties scored. Each one is also counted in <see cref="Goals"/>,
    /// so <see cref="Goals"/> is never lower than this value.
    /// </summary>
    public int PenaltiesScored { get; init; }

    /// <summary>
    /// The number of penalties missed.
    /// </summary>
    public int PenaltiesMissed { get; init; }

    /// <summary>
    /// The number of yellow cards received.
    /// </summary>
    public int YellowCards { get; init; }

    /// <summary>
    /// The number of red cards received.
    /// </summary>
    public int RedCards { get; init; }
}
