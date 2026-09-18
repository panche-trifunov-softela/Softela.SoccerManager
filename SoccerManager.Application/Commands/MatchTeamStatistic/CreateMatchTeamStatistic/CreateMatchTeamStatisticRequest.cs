using MediatR;

namespace SoccerManager.Application.Commands.MatchTeamStatistic.CreateMatchTeamStatistic;

/// <summary>
/// Represents a request to create a new match team statistic.
/// </summary>
public sealed record CreateMatchTeamStatisticRequest : IRequest<int>
{
    /// <summary>
    /// The identifier of the team the statistics belong to.
    /// </summary>
    public int TeamId { get; init; }

    /// <summary>
    /// The identifier of the match the statistics belong to.
    /// </summary>
    public int MatchId { get; init; }

    /// <summary>
    /// Whether the team played the match at home.
    /// </summary>
    public bool IsHomeTeam { get; init; }

    /// <summary>
    /// The number of shots taken, on target or not.
    /// </summary>
    public int ShotsTotal { get; init; }

    /// <summary>
    /// The number of shots on target. These are a subset of <see cref="ShotsTotal"/>,
    /// so this value is never greater than it.
    /// </summary>
    public int ShotsOnTarget { get; init; }

    /// <summary>
    /// The share of possession as a whole-number percentage, from 0 to 100 inclusive.
    /// </summary>
    public int Possession { get; init; }
}
