using MediatR;

namespace SoccerManager.Application.Queries.Division.GetDivisionById;

/// <summary>
/// Represents a request to retrieve a single division by identifier.
/// </summary>
public sealed record GetDivisionByIdRequest : IRequest<GetDivisionByIdResponse>
{
    /// <summary>
    /// The identifier of the division to retrieve.
    /// </summary>
    public int Id { get; init; }
}
