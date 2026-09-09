using MediatR;

namespace SoccerManager.Application.Queries.Team.GetTeamById;

/// <summary>
/// Represents a request to retrieve a single team by identifier.
/// </summary>
public sealed record GetTeamByIdRequest : IRequest<GetTeamByIdResponse>
{
    /// <summary>
    /// The identifier of the team to retrieve.
    /// </summary>
    public int Id { get; init; }
}
