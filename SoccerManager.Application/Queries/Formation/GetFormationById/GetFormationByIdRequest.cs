using MediatR;

namespace SoccerManager.Application.Queries.Formation.GetFormationById;

/// <summary>
/// Represents a request to retrieve a single formation by identifier.
/// </summary>
public sealed record GetFormationByIdRequest : IRequest<GetFormationByIdResponse>
{
    /// <summary>
    /// The identifier of the formation to retrieve.
    /// </summary>
    public int Id { get; init; }
}
