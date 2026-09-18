using MediatR;

namespace SoccerManager.Application.Queries.MatchPlayerStatistic.GetMatchPlayerStatisticById;

/// <summary>
/// Represents a request to retrieve a single match player statistic by identifier.
/// </summary>
public sealed record GetMatchPlayerStatisticByIdRequest : IRequest<GetMatchPlayerStatisticByIdResponse>
{
    /// <summary>
    /// The identifier of the match player statistic to retrieve.
    /// </summary>
    public int Id { get; init; }
}
