using MediatR;

namespace SoccerManager.Application.Queries.League.GetLeagueById;

/// <summary>
/// Represents a request to retrieve a single league by identifier.
/// </summary>
public sealed record GetLeagueByIdRequest : IRequest<GetLeagueByIdResponse>
{
    /// <summary>
    /// The identifier of the league to retrieve.
    /// </summary>
    public int Id { get; init; }
}
