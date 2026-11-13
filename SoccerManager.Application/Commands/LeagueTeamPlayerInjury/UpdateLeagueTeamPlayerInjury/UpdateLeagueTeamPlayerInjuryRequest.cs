using MediatR;
using SoccerManager.Domain.Enums;

namespace SoccerManager.Application.Commands.LeagueTeamPlayerInjury.UpdateLeagueTeamPlayerInjury;

/// <summary>
/// Represents a request to update an existing injury of a squad row.
/// </summary>
public sealed record UpdateLeagueTeamPlayerInjuryRequest : IRequest<bool>
{
    /// <summary>
    /// The identifier of the injury to update.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// The identifier of the squad row (the player's registration at the club) the injury is recorded on.
    /// </summary>
    public int LeagueTeamPlayerId { get; init; }

    /// <summary>
    /// The name of the injury, as shown to the user.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// The UTC date and time the injury started.
    /// </summary>
    public DateTime InjuryStartDate { get; init; }

    /// <summary>
    /// The UTC date and time the injury ended. It is never before <see cref="InjuryStartDate"/>.
    /// </summary>
    public DateTime InjuryEndDate { get; init; }

    /// <summary>
    /// How serious the injury was.
    /// </summary>
    public InjuryScale InjuryScale { get; init; }
}
