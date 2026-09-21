using MediatR;

namespace SoccerManager.Application.Queries.FormationPosition.GetFormationPositionById;

/// <summary>
/// Represents a request to retrieve a single formation position slot by identifier.
/// </summary>
public sealed record GetFormationPositionByIdRequest : IRequest<GetFormationPositionByIdResponse>
{
    /// <summary>
    /// The identifier of the formation position slot to retrieve.
    /// </summary>
    public int Id { get; init; }
}
