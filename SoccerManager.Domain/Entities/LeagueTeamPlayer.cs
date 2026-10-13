using SoccerManager.Domain.Enums;

namespace SoccerManager.Domain.Entities;

/// <summary>
/// Represents a player's registration with a club on a league's roster, together with the contract and
/// squad details that go with it.
/// </summary>
public class LeagueTeamPlayer : BaseEntity
{
    /// <summary>
    /// Gets or sets the identifier of the league team (the club's roster entry in a league) the player
    /// is registered with. It identifies both the league and the team.
    /// </summary>
    public int LeagueTeamId { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the registered player.
    /// </summary>
    public int PlayerId { get; set; }

    /// <summary>
    /// Gets or sets the length of the contract in years, from 1 to 6 inclusive.
    /// </summary>
    public int ContractLength { get; set; }

    /// <summary>
    /// Gets or sets the weekly salary the contract pays.
    /// </summary>
    public long ContractSalaryPerWeek { get; set; }

    /// <summary>
    /// Gets or sets the player's squad number, from 1 to 99 inclusive.
    /// </summary>
    public int SquadNumber { get; set; }

    /// <summary>
    /// Gets or sets whether the player is a favourite of the fans.
    /// </summary>
    public bool FansFavoritePlayer { get; set; }

    /// <summary>
    /// Gets or sets the player's morale.
    /// </summary>
    public Morale Morale { get; set; }

    /// <summary>
    /// Gets or sets the player's value on the transfer market.
    /// </summary>
    public long TransfermarketValue { get; set; }

    /// <summary>
    /// Gets or sets the number of starting appearances the player wants over the contract.
    /// </summary>
    public int WantedStarterAppearances { get; set; }

    /// <summary>
    /// Gets or sets the total number of appearances the player wants over the contract. These include
    /// the starting ones, so this value is never lower than <see cref="WantedStarterAppearances"/>.
    /// </summary>
    public int WantedTotalAppearances { get; set; }

    /// <summary>
    /// Gets or sets the player's physical condition, from 1 to 100 inclusive.
    /// </summary>
    public int Condition { get; set; }

    /// <summary>
    /// Gets or sets whether the player is suspended from the domestic competition.
    /// </summary>
    public bool IsSuspendedDomesticCompetition { get; set; }

    /// <summary>
    /// Gets or sets whether the player is suspended from the continental competition.
    /// </summary>
    public bool IsSuspendedContinentalCompetition { get; set; }
}
