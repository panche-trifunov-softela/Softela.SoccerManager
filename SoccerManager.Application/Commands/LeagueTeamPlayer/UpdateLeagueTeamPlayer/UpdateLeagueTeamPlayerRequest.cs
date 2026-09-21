using MediatR;
using SoccerManager.Domain.Enums;

namespace SoccerManager.Application.Commands.LeagueTeamPlayer.UpdateLeagueTeamPlayer;

/// <summary>
/// Represents a request to update an existing league team player.
/// </summary>
public sealed record UpdateLeagueTeamPlayerRequest : IRequest<bool>
{
    /// <summary>
    /// The identifier of the league team player to update.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// The identifier of the league the registration belongs to.
    /// </summary>
    public int LeagueId { get; init; }

    /// <summary>
    /// The identifier of the team the player is registered with.
    /// </summary>
    public int TeamId { get; init; }

    /// <summary>
    /// The identifier of the registered player.
    /// </summary>
    public int PlayerId { get; init; }

    /// <summary>
    /// The length of the contract in years, from 1 to 6 inclusive.
    /// </summary>
    public int ContractLength { get; init; }

    /// <summary>
    /// The weekly salary the contract pays.
    /// </summary>
    public long ContractSalaryPerWeek { get; init; }

    /// <summary>
    /// The player's squad number, from 1 to 99 inclusive.
    /// </summary>
    public int SquadNumber { get; init; }

    /// <summary>
    /// Whether the player is a favourite of the fans.
    /// </summary>
    public bool FansFavoritePlayer { get; init; }

    /// <summary>
    /// The player's morale.
    /// </summary>
    public Morale Morale { get; init; }

    /// <summary>
    /// The player's value on the transfer market.
    /// </summary>
    public long TransfermarketValue { get; init; }

    /// <summary>
    /// The number of starting appearances the player wants over the contract.
    /// </summary>
    public int WantedStarterAppearances { get; init; }

    /// <summary>
    /// The total number of appearances the player wants over the contract. These include the
    /// starting ones, so this value is never lower than <see cref="WantedStarterAppearances"/>.
    /// </summary>
    public int WantedTotalAppearances { get; init; }

    /// <summary>
    /// The player's physical condition, from 1 to 100.
    /// </summary>
    public int Condition { get; init; }

    /// <summary>
    /// Whether the player is suspended from the domestic competition.
    /// </summary>
    public bool IsSuspendedDomesticCompetition { get; init; }

    /// <summary>
    /// Whether the player is suspended from the continental competition.
    /// </summary>
    public bool IsSuspendedContinentalCompetition { get; init; }
}
