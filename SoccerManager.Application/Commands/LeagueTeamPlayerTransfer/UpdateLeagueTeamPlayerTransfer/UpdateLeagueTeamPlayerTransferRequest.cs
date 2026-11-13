using MediatR;
using SoccerManager.Domain.Enums;

namespace SoccerManager.Application.Commands.LeagueTeamPlayerTransfer.UpdateLeagueTeamPlayerTransfer;

/// <summary>
/// Represents a request to update an existing transfer of a squad row. The selling club is not part of the request: the
/// handler keeps it, or takes it from the new squad row when the request names another one.
/// </summary>
public sealed record UpdateLeagueTeamPlayerTransferRequest : IRequest<bool>
{
    /// <summary>
    /// The identifier of the transfer to update.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// The identifier of the squad row (the player's registration at the selling club) that is transferred.
    /// </summary>
    public int LeagueTeamPlayerId { get; init; }

    /// <summary>
    /// The identifier of the league team (the club in its league) that buys the player.
    /// </summary>
    public int ToLeagueTeamId { get; init; }

    /// <summary>
    /// The identifier of the season the transfer takes place in.
    /// </summary>
    public int SeasonId { get; init; }

    /// <summary>
    /// The fee the buying club pays, which is zero for a free transfer.
    /// </summary>
    public decimal TransferFee { get; init; }

    /// <summary>
    /// The price at which the selling club can buy the player back, or <see langword="null"/> when there is no buyback.
    /// It is set together with <see cref="BuybackValidSeasons"/> or not at all.
    /// </summary>
    public decimal? BuybackValue { get; init; }

    /// <summary>
    /// The number of seasons the buyback stays valid, or <see langword="null"/> when there is no buyback. It is set
    /// together with <see cref="BuybackValue"/> or not at all.
    /// </summary>
    public int? BuybackValidSeasons { get; init; }

    /// <summary>
    /// Where the transfer stands.
    /// </summary>
    public TransferStatus Status { get; init; }
}
