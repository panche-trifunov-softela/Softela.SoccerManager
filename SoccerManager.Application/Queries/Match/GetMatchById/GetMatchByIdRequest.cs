using MediatR;

namespace SoccerManager.Application.Queries.Match.GetMatchById;

/// <summary>
/// Represents a request to retrieve a single match by identifier.
/// </summary>
public sealed record GetMatchByIdRequest : IRequest<GetMatchByIdResponse>
{
    /// <summary>
    /// The identifier of the match to retrieve.
    /// </summary>
    public int Id { get; init; }
}
