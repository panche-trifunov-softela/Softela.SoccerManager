using MediatR;

namespace SoccerManager.Application.Commands.MatchPlayerStatistic.DeleteMatchPlayerStatistic;

/// <summary>
/// Represents a request to delete an existing match player statistic.
/// </summary>
public sealed record DeleteMatchPlayerStatisticRequest : IRequest<bool>
{
    /// <summary>
    /// The identifier of the match player statistic to delete.
    /// </summary>
    public int Id { get; init; }
}
