using MediatR;

namespace SoccerManager.Application.Queries.FormationPosition.GetFormationPositions;

/// <summary>
/// Represents a request to retrieve every position slot belonging to a formation.
/// </summary>
public sealed record GetFormationPositionsRequest : IRequest<GetFormationPositionsResponse>
{
    /// <summary>
    /// The identifier of the formation whose position slots are being requested.
    /// </summary>
    public int FormationId { get; init; }
}
