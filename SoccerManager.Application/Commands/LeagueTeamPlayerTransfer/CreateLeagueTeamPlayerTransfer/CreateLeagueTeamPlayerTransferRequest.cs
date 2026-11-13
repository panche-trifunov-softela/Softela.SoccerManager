using MediatR;
using SoccerManager.Domain.Enums;

namespace SoccerManager.Application.Commands.LeagueTeamPlayerTransfer.CreateLeagueTeamPlayerTransfer;

/// <summary>
/// Represents a request to create a new transfer of a squad row. The selling club is not part of the request: the handler
/// takes it from the squad row.
/// </summary>
public sealed record CreateLeagueTeamPlayerTransferRequest : IRequest<int>
{
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
