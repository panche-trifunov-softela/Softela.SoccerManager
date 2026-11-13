using MediatR;

namespace SoccerManager.Application.Commands.LeagueTeamPlayerInjury.DeleteLeagueTeamPlayerInjury;

/// <summary>
/// Represents a request to delete an existing injury of a squad row.
/// </summary>
public sealed record DeleteLeagueTeamPlayerInjuryRequest : IRequest<bool>
{
    /// <summary>
    /// The identifier of the injury to delete.
    /// </summary>
    public int Id { get; init; }
}
