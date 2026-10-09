using MediatR;

namespace SoccerManager.Application.Queries.LeagueTeamManagerApplication.GetLeagueTeamManagerApplicationById;

/// <summary>
/// Represents a request to retrieve a single league team manager application by identifier.
/// </summary>
public sealed record GetLeagueTeamManagerApplicationByIdRequest : IRequest<GetLeagueTeamManagerApplicationByIdResponse>
{
    /// <summary>
    /// The identifier of the league team manager application to retrieve.
    /// </summary>
    public int Id { get; init; }
}
