using MediatR;

namespace SoccerManager.Application.Queries.MatchTeamStatistic.GetMatchTeamStatisticById;

/// <summary>
/// Represents a request to retrieve a single match team statistic by identifier.
/// </summary>
public sealed record GetMatchTeamStatisticByIdRequest : IRequest<GetMatchTeamStatisticByIdResponse>
{
    /// <summary>
    /// The identifier of the match team statistic to retrieve.
    /// </summary>
    public int Id { get; init; }
}
