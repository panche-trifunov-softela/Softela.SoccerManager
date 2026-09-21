using MediatR;

namespace SoccerManager.Application.Queries.MatchFormationPlayerPosition.GetMatchFormationPlayerPositionById;

/// <summary>
/// Represents a request to retrieve a single match formation player position by identifier.
/// </summary>
public sealed record GetMatchFormationPlayerPositionByIdRequest : IRequest<GetMatchFormationPlayerPositionByIdResponse>
{
    /// <summary>
    /// The identifier of the match formation player position to retrieve.
    /// </summary>
    public int Id { get; init; }
}
