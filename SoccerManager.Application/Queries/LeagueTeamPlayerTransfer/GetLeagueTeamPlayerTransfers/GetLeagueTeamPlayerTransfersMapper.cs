using SoccerManager.Application.Dtos;

namespace SoccerManager.Application.Queries.LeagueTeamPlayerTransfer.GetLeagueTeamPlayerTransfers;

/// <summary>
/// Maps transfer domain entities to <see cref="LeagueTeamPlayerTransferDto"/> instances.
/// </summary>
public static class GetLeagueTeamPlayerTransfersMapper
{
    /// <summary>
    /// Converts a transfer entity into its DTO representation.
    /// </summary>
    /// <param name="leagueTeamPlayerTransfer">The transfer entity to convert.</param>
    /// <returns>The corresponding <see cref="LeagueTeamPlayerTransferDto"/>.</returns>
    public static LeagueTeamPlayerTransferDto ToDto(SoccerManager.Domain.Entities.LeagueTeamPlayerTransfer leagueTeamPlayerTransfer)
    {
        return new LeagueTeamPlayerTransferDto
        {
            Id = leagueTeamPlayerTransfer.Id,
            LeagueTeamPlayerId = leagueTeamPlayerTransfer.LeagueTeamPlayerId,
            FromLeagueTeamId = leagueTeamPlayerTransfer.FromLeagueTeamId,
            ToLeagueTeamId = leagueTeamPlayerTransfer.ToLeagueTeamId,
            SeasonId = leagueTeamPlayerTransfer.SeasonId,
            TransferFee = leagueTeamPlayerTransfer.TransferFee,
            BuybackValue = leagueTeamPlayerTransfer.BuybackValue,
            BuybackValidSeasons = leagueTeamPlayerTransfer.BuybackValidSeasons,
            Status = leagueTeamPlayerTransfer.Status,
            CreatedAt = leagueTeamPlayerTransfer.CreatedAt,
            ModifiedAt = leagueTeamPlayerTransfer.ModifiedAt,
        };
    }
}
