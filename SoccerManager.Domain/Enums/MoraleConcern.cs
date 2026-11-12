namespace SoccerManager.Domain.Enums;

/// <summary>
/// Represents the reason behind a player's morale at the team, as stored in <c>dbo.LeagueTeamPlayers.MoraleConcern</c>.
/// A player with no particular concern has no value at all. The values deliberately start at 1, like <see cref="Morale"/>,
/// and <c>CK_LeagueTeamPlayers_MoraleConcernKnown</c> accepts exactly these values, so a new member needs a new script
/// as well.
/// </summary>
public enum MoraleConcern : byte
{
    /// <summary>
    /// The player is unhappy with how little they play.
    /// </summary>
    PlayingTime = 1,

    /// <summary>
    /// The player is unhappy with their squad number.
    /// </summary>
    SquadNumber = 2,

    /// <summary>
    /// The player is unhappy with the length of their contract.
    /// </summary>
    ContractLength = 3,

    /// <summary>
    /// The player is unhappy with their wage.
    /// </summary>
    Wage = 4,

    /// <summary>
    /// The player is unhappy that they have been put on the transfer list.
    /// </summary>
    TransferListed = 5,
}
