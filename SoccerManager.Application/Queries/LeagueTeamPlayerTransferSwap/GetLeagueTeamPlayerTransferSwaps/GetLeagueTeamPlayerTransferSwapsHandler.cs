using MediatR;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Queries.LeagueTeamPlayerTransferSwap.GetLeagueTeamPlayerTransferSwaps;

/// <summary>
/// Handles <see cref="GetLeagueTeamPlayerTransferSwapsRequest"/> queries.
/// </summary>
public class GetLeagueTeamPlayerTransferSwapsHandler : IRequestHandler<GetLeagueTeamPlayerTransferSwapsRequest, GetLeagueTeamPlayerTransferSwapsResponse>
{
    private readonly ILeagueTeamPlayerTransferSwapRepository _leagueTeamPlayerTransferSwapRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetLeagueTeamPlayerTransferSwapsHandler"/> class.
    /// </summary>
    /// <param name="leagueTeamPlayerTransferSwapRepository">The repository used to load swaps.</param>
    public GetLeagueTeamPlayerTransferSwapsHandler(ILeagueTeamPlayerTransferSwapRepository leagueTeamPlayerTransferSwapRepository)
    {
        _leagueTeamPlayerTransferSwapRepository = leagueTeamPlayerTransferSwapRepository;
    }

    /// <summary>
    /// Retrieves every swapped player of the requested transfer.
    /// </summary>
    /// <param name="request">The query request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The response carrying the matching swaps.</returns>
    public async Task<GetLeagueTeamPlayerTransferSwapsResponse> Handle(GetLeagueTeamPlayerTransferSwapsRequest request, CancellationToken cancellationToken)
    {
        // No transfer-existence check here: a filter matching nothing is not an
        // error, just an empty list, and this keeps ILeagueTeamPlayerTransferRepository out of a
        // handler that only needs ILeagueTeamPlayerTransferSwapRepository.
        var leagueTeamPlayerTransferSwaps = await _leagueTeamPlayerTransferSwapRepository.GetByLeagueTeamPlayerTransferIdAsync(request.LeagueTeamPlayerTransferId);

        return new GetLeagueTeamPlayerTransferSwapsResponse { Data = leagueTeamPlayerTransferSwaps.Select(GetLeagueTeamPlayerTransferSwapsMapper.ToDto).ToList() };
    }
}
