using MediatR;

namespace SoccerManager.Application.Queries.Stadium.GetStadiumById;

/// <summary>
/// Represents a request to retrieve a single stadium by identifier.
/// </summary>
public sealed record GetStadiumByIdRequest : IRequest<GetStadiumByIdResponse>
{
    /// <summary>
    /// The identifier of the stadium to retrieve.
    /// </summary>
    public int Id { get; init; }
}
