using MediatR;

namespace SoccerManager.Application.Queries.LeagueTeamPlayerInjury.GetLeagueTeamPlayerInjuryById;

/// <summary>
/// Represents a request to retrieve a single injury by identifier.
/// </summary>
public sealed record GetLeagueTeamPlayerInjuryByIdRequest : IRequest<GetLeagueTeamPlayerInjuryByIdResponse>
{
    /// <summary>
    /// The identifier of the injury to retrieve.
    /// </summary>
    public int Id { get; init; }
}
