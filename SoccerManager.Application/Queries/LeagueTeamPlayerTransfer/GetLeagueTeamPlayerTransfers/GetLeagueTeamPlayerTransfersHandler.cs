using MediatR;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Queries.LeagueTeamPlayerTransfer.GetLeagueTeamPlayerTransfers;

/// <summary>
/// Handles <see cref="GetLeagueTeamPlayerTransfersRequest"/> queries.
/// </summary>
public class GetLeagueTeamPlayerTransfersHandler : IRequestHandler<GetLeagueTeamPlayerTransfersRequest, GetLeagueTeamPlayerTransfersResponse>
{
    private readonly ILeagueTeamPlayerTransferRepository _leagueTeamPlayerTransferRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetLeagueTeamPlayerTransfersHandler"/> class.
    /// </summary>
    /// <param name="leagueTeamPlayerTransferRepository">The repository used to load transfers.</param>
    public GetLeagueTeamPlayerTransfersHandler(ILeagueTeamPlayerTransferRepository leagueTeamPlayerTransferRepository)
    {
        _leagueTeamPlayerTransferRepository = leagueTeamPlayerTransferRepository;
    }

    /// <summary>
    /// Retrieves every transfer of the requested squad row.
    /// </summary>
    /// <param name="request">The query request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The response carrying the matching transfers.</returns>
    public async Task<GetLeagueTeamPlayerTransfersResponse> Handle(GetLeagueTeamPlayerTransfersRequest request, CancellationToken cancellationToken)
    {
        // No squad-row-existence check here: a filter matching nothing is not an
        // error, just an empty list, and this keeps ILeagueTeamPlayerRepository out of a
        // handler that only needs ILeagueTeamPlayerTransferRepository.
        var leagueTeamPlayerTransfers = await _leagueTeamPlayerTransferRepository.GetByLeagueTeamPlayerIdAsync(request.LeagueTeamPlayerId);

        return new GetLeagueTeamPlayerTransfersResponse { Data = leagueTeamPlayerTransfers.Select(GetLeagueTeamPlayerTransfersMapper.ToDto).ToList() };
    }
}
