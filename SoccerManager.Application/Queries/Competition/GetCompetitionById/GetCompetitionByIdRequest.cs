using MediatR;

namespace SoccerManager.Application.Queries.Competition.GetCompetitionById;

/// <summary>
/// Represents a request to retrieve a single competition by identifier.
/// </summary>
public sealed record GetCompetitionByIdRequest : IRequest<GetCompetitionByIdResponse>
{
    /// <summary>
    /// The identifier of the competition to retrieve.
    /// </summary>
    public int Id { get; init; }
}
