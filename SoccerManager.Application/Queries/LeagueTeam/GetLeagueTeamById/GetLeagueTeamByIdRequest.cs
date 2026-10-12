using MediatR;

namespace SoccerManager.Application.Queries.LeagueTeam.GetLeagueTeamById;

/// <summary>
/// Represents a request to retrieve a single league team by identifier.
/// </summary>
public sealed record GetLeagueTeamByIdRequest : IRequest<GetLeagueTeamByIdResponse>
{
    /// <summary>
    /// The identifier of the league team to retrieve.
    /// </summary>
    public int Id { get; init; }
}
