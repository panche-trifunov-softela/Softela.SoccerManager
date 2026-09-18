using MediatR;

namespace SoccerManager.Application.Commands.MatchTeamStatistic.DeleteMatchTeamStatistic;

/// <summary>
/// Represents a request to delete an existing match team statistic.
/// </summary>
public sealed record DeleteMatchTeamStatisticRequest : IRequest<bool>
{
    /// <summary>
    /// The identifier of the match team statistic to delete.
    /// </summary>
    public int Id { get; init; }
}
