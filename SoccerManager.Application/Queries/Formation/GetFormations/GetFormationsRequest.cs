using MediatR;

namespace SoccerManager.Application.Queries.Formation.GetFormations;

/// <summary>
/// Represents a request to retrieve all formations.
/// </summary>
public sealed record GetFormationsRequest : IRequest<GetFormationsResponse>;
