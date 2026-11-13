using MediatR;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Queries.LeagueTeamPlayerTransfer.GetLeagueTeamPlayerTransferById;

/// <summary>
/// Handles <see cref="GetLeagueTeamPlayerTransferByIdRequest"/> queries.
/// </summary>
public class GetLeagueTeamPlayerTransferByIdHandler : IRequestHandler<GetLeagueTeamPlayerTransferByIdRequest, GetLeagueTeamPlayerTransferByIdResponse>
{
    private readonly ILeagueTeamPlayerTransferRepository _leagueTeamPlayerTransferRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetLeagueTeamPlayerTransferByIdHandler"/> class.
    /// </summary>
    /// <param name="leagueTeamPlayerTransferRepository">The repository used to load transfers.</param>
    public GetLeagueTeamPlayerTransferByIdHandler(ILeagueTeamPlayerTransferRepository leagueTeamPlayerTransferRepository)
    {
        _leagueTeamPlayerTransferRepository = leagueTeamPlayerTransferRepository;
    }

    /// <summary>
    /// Retrieves the transfer identified by the given request.
    /// </summary>
    /// <param name="request">The query request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The response carrying the requested transfer.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when no transfer with the given identifier exists.</exception>
    public async Task<GetLeagueTeamPlayerTransferByIdResponse> Handle(GetLeagueTeamPlayerTransferByIdRequest request, CancellationToken cancellationToken)
    {
        var leagueTeamPlayerTransfer = await _leagueTeamPlayerTransferRepository.GetByIdAsync(request.Id)
            ?? throw new KeyNotFoundException($"League team player transfer {request.Id} not found.");

        return new GetLeagueTeamPlayerTransferByIdResponse { Data = GetLeagueTeamPlayerTransferByIdMapper.ToDto(leagueTeamPlayerTransfer) };
    }
}
