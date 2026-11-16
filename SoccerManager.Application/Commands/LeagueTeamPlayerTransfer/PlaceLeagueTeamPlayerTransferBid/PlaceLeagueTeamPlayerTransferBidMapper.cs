using SoccerManager.Application.Services;
using SoccerManager.Domain.Enums;

namespace SoccerManager.Application.Commands.LeagueTeamPlayerTransfer.PlaceLeagueTeamPlayerTransferBid;

/// <summary>
/// Maps <see cref="PlaceLeagueTeamPlayerTransferBidRequest"/> instances to domain entities.
/// </summary>
public static class PlaceLeagueTeamPlayerTransferBidMapper
{
    /// <summary>
    /// Creates a new pending transfer entity from the given request. The squad row, the selling club and the season come
    /// from the resolved target, never from the caller.
    /// </summary>
    /// <param name="request">The request carrying the bid values.</param>
    /// <param name="target">The resolved target of the bid.</param>
    /// <param name="now">The UTC timestamp to stamp on the created and modified fields.</param>
    /// <param name="userId">The identifier of the user placing the bid.</param>
    /// <returns>A new, unsaved transfer entity.</returns>
    public static SoccerManager.Domain.Entities.LeagueTeamPlayerTransfer ToDomainEntity(PlaceLeagueTeamPlayerTransferBidRequest request, BidTarget target, DateTime now, Guid userId)
    {
        return new SoccerManager.Domain.Entities.LeagueTeamPlayerTransfer
        {
            LeagueTeamPlayerId = target.LeagueTeamPlayerId,
            FromLeagueTeamId = target.FromLeagueTeamId,
            ToLeagueTeamId = request.LeagueTeamId,
            SeasonId = target.SeasonId,
            TransferFee = request.TransferFee,
            BuybackValue = request.BuybackValue,
            BuybackValidSeasons = request.BuybackValidSeasons,
            Status = TransferStatus.Pending,
            CreatedAt = now,
            CreatedBy = userId,
            ModifiedAt = now,
            ModifiedBy = userId,
        };
    }

    /// <summary>
    /// Creates a new swap entity that offers a squad player of the bidding club in the given bid.
    /// </summary>
    /// <param name="leagueTeamPlayerTransferId">The identifier of the transfer the player is offered in.</param>
    /// <param name="leagueTeamPlayerId">The identifier of the squad row (the player at the bidding club) that is offered.</param>
    /// <param name="now">The UTC timestamp to stamp on the created and modified fields.</param>
    /// <param name="userId">The identifier of the user placing the bid.</param>
    /// <returns>A new, unsaved swap entity.</returns>
    public static SoccerManager.Domain.Entities.LeagueTeamPlayerTransferSwap ToSwap(int leagueTeamPlayerTransferId, int leagueTeamPlayerId, DateTime now, Guid userId)
    {
        return new SoccerManager.Domain.Entities.LeagueTeamPlayerTransferSwap
        {
            LeagueTeamPlayerTransferId = leagueTeamPlayerTransferId,
            LeagueTeamPlayerId = leagueTeamPlayerId,
            CreatedAt = now,
            CreatedBy = userId,
            ModifiedAt = now,
            ModifiedBy = userId,
        };
    }
}
