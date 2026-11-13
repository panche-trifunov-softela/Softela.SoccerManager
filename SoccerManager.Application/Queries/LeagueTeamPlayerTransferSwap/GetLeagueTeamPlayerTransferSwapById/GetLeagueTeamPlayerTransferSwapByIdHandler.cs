using MediatR;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Queries.LeagueTeamPlayerTransferSwap.GetLeagueTeamPlayerTransferSwapById;

/// <summary>
/// Handles <see cref="GetLeagueTeamPlayerTransferSwapByIdRequest"/> queries.
/// </summary>
public class GetLeagueTeamPlayerTransferSwapByIdHandler : IRequestHandler<GetLeagueTeamPlayerTransferSwapByIdRequest, GetLeagueTeamPlayerTransferSwapByIdResponse>
{
    private readonly ILeagueTeamPlayerTransferSwapRepository _leagueTeamPlayerTransferSwapRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetLeagueTeamPlayerTransferSwapByIdHandler"/> class.
    /// </summary>
    /// <param name="leagueTeamPlayerTransferSwapRepository">The repository used to load swaps.</param>
    public GetLeagueTeamPlayerTransferSwapByIdHandler(ILeagueTeamPlayerTransferSwapRepository leagueTeamPlayerTransferSwapRepository)
    {
        _leagueTeamPlayerTransferSwapRepository = leagueTeamPlayerTransferSwapRepository;
    }

    /// <summary>
    /// Retrieves the swap identified by the given request.
    /// </summary>
    /// <param name="request">The query request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The response carrying the requested swap.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when no swap with the given identifier exists.</exception>
    public async Task<GetLeagueTeamPlayerTransferSwapByIdResponse> Handle(GetLeagueTeamPlayerTransferSwapByIdRequest request, CancellationToken cancellationToken)
    {
        var leagueTeamPlayerTransferSwap = await _leagueTeamPlayerTransferSwapRepository.GetByIdAsync(request.Id)
            ?? throw new KeyNotFoundException($"League team player transfer swap {request.Id} not found.");

        return new GetLeagueTeamPlayerTransferSwapByIdResponse { Data = GetLeagueTeamPlayerTransferSwapByIdMapper.ToDto(leagueTeamPlayerTransferSwap) };
    }
}
