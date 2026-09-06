using MediatR;

namespace SoccerManager.Application.Queries.Season.GetSeasonById;

/// <summary>
/// Represents a request to retrieve a single season by identifier.
/// </summary>
public sealed record GetSeasonByIdRequest : IRequest<GetSeasonByIdResponse>
{
    /// <summary>
    /// The identifier of the season to retrieve.
    /// </summary>
    public int Id { get; init; }
}
