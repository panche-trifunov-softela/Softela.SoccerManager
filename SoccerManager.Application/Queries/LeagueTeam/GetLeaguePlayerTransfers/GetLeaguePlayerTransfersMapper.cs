using SoccerManager.Application.Dtos;
using SoccerManager.Application.Models;

namespace SoccerManager.Application.Queries.LeagueTeam.GetLeaguePlayerTransfers;

/// <summary>
/// Maps a player's transfer and loan rows to <see cref="PlayerTransferDto"/> instances.
/// </summary>
public static class GetLeaguePlayerTransfersMapper
{
    /// <summary>
    /// Converts the deal rows and the swapped-player rows into the transfers' DTO representation, each transfer carrying its
    /// own swapped players in row order and each loan an empty list.
    /// </summary>
    /// <param name="rows">The deal rows returned by the stored procedure, in the order to keep.</param>
    /// <param name="swapPlayers">The swapped-player rows returned by the stored procedure, in swap-row order.</param>
    /// <returns>The corresponding <see cref="PlayerTransferDto"/> list, in the order of <paramref name="rows"/>.</returns>
    public static List<PlayerTransferDto> ToDtos(IReadOnlyList<GetLeaguePlayerTransfersResult> rows, IReadOnlyList<GetLeaguePlayerTransferSwapPlayersResult> swapPlayers)
    {
        var swapPlayersByTransfer = swapPlayers.ToLookup(x => x.LeagueTeamPlayerTransferId);

        return rows.Select(row => ToDto(row, row.IsLoan ? new List<GetLeaguePlayerTransferSwapPlayersResult>() : swapPlayersByTransfer[row.Id].ToList())).ToList();
    }

    /// <summary>
    /// Converts a deal row and the players swapped in it into its DTO representation.
    /// </summary>
    /// <param name="row">The deal row returned by the stored procedure.</param>
    /// <param name="swapPlayers">The swapped-player rows of this deal, empty for a loan.</param>
    /// <returns>The corresponding <see cref="PlayerTransferDto"/>.</returns>
    public static PlayerTransferDto ToDto(GetLeaguePlayerTransfersResult row, IReadOnlyList<GetLeaguePlayerTransferSwapPlayersResult> swapPlayers)
    {
        return new PlayerTransferDto
        {
            Id = row.Id,
            IsLoan = row.IsLoan,
            LeagueTeamId = row.LeagueTeamId,
            TeamName = row.TeamName,
            TeamLogoUrl = row.TeamLogoUrl,
            SeasonId = row.SeasonId,
            SeasonNumber = row.SeasonNumber,
            Fee = row.Fee,
            PlayerId = row.PlayerId,
            PlayerName = row.PlayerName,
            SwapPlayers = swapPlayers.Select(ToSwapPlayerDto).ToList(),
        };
    }

    /// <summary>
    /// Converts a swapped-player row into its DTO representation.
    /// </summary>
    /// <param name="row">The swapped-player row returned by the stored procedure.</param>
    /// <returns>The corresponding <see cref="PlayerTransferSwapPlayerDto"/>.</returns>
    public static PlayerTransferSwapPlayerDto ToSwapPlayerDto(GetLeaguePlayerTransferSwapPlayersResult row)
    {
        return new PlayerTransferSwapPlayerDto
        {
            PlayerId = row.PlayerId,
            Name = row.Name,
        };
    }
}
