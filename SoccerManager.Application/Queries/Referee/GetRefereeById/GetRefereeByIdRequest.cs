using MediatR;

namespace SoccerManager.Application.Queries.Referee.GetRefereeById;

/// <summary>
/// Represents a request to retrieve a single referee by identifier.
/// </summary>
public sealed record GetRefereeByIdRequest : IRequest<GetRefereeByIdResponse>
{
    /// <summary>
    /// The identifier of the referee to retrieve.
    /// </summary>
    public int Id { get; init; }
}
