namespace SoccerManager.Application.Services;

/// <summary>
/// Represents what a club's bid for a player resolves to: the squad row it bids for, the club that sells or lends the player,
/// the season the bid is placed in and the bidding club's own squad rows.
/// </summary>
public sealed record BidTarget
{
    /// <summary>
    /// The identifier of the squad row (the player's registration at the selling club) the bid is for.
    /// </summary>
    public int LeagueTeamPlayerId { get; init; }

    /// <summary>
    /// The identifier of the league team (the club in its league) the player is registered at.
    /// </summary>
    public int FromLeagueTeamId { get; init; }

    /// <summary>
    /// The identifier of the league's latest season, the one the bid is placed in.
    /// </summary>
    public int SeasonId { get; init; }

    /// <summary>
    /// The identifiers of the bidding club's squad rows, which the players offered in a swap must come from.
    /// </summary>
    public IReadOnlyList<int> SquadLeagueTeamPlayerIds { get; init; } = [];
}
