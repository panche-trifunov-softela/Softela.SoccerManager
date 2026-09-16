using MediatR;

namespace SoccerManager.Application.Queries.LeagueTeamManager.GetLeagueTeamManagerById;

/// <summary>
/// Represents a request to retrieve a single league team manager appointment by identifier.
/// </summary>
public sealed record GetLeagueTeamManagerByIdRequest : IRequest<GetLeagueTeamManagerByIdResponse>
{
    /// <summary>
    /// The identifier of the league team manager appointment to retrieve.
    /// </summary>
    public int Id { get; init; }
}
