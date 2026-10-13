using MediatR;

namespace SoccerManager.Application.Commands.LeagueTeamManagerApplication.CreateLeagueTeamManagerApplication;

/// <summary>
/// Represents a request by the authenticated caller to apply to manage a team in a league. The applying
/// manager is the caller's own manager profile, so it is deliberately not part of the request.
/// </summary>
public sealed record CreateLeagueTeamManagerApplicationRequest : IRequest<int>
{
    /// <summary>
    /// The identifier of the league roster entry (the club in its league) the caller applies to manage.
    /// </summary>
    public int LeagueTeamId { get; init; }
}
