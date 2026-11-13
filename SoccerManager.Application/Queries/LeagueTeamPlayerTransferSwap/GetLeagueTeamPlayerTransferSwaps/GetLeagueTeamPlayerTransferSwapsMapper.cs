using SoccerManager.Application.Dtos;

namespace SoccerManager.Application.Queries.LeagueTeamPlayerTransferSwap.GetLeagueTeamPlayerTransferSwaps;

/// <summary>
/// Maps swap domain entities to <see cref="LeagueTeamPlayerTransferSwapDto"/> instances.
/// </summary>
public static class GetLeagueTeamPlayerTransferSwapsMapper
{
    /// <summary>
    /// Converts a swap entity into its DTO representation.
    /// </summary>
    /// <param name="leagueTeamPlayerTransferSwap">The swap entity to convert.</param>
    /// <returns>The corresponding <see cref="LeagueTeamPlayerTransferSwapDto"/>.</returns>
    public static LeagueTeamPlayerTransferSwapDto ToDto(SoccerManager.Domain.Entities.LeagueTeamPlayerTransferSwap leagueTeamPlayerTransferSwap)
    {
        return new LeagueTeamPlayerTransferSwapDto
        {
            Id = leagueTeamPlayerTransferSwap.Id,
            LeagueTeamPlayerTransferId = leagueTeamPlayerTransferSwap.LeagueTeamPlayerTransferId,
            LeagueTeamPlayerId = leagueTeamPlayerTransferSwap.LeagueTeamPlayerId,
            CreatedAt = leagueTeamPlayerTransferSwap.CreatedAt,
            ModifiedAt = leagueTeamPlayerTransferSwap.ModifiedAt,
        };
    }
}
