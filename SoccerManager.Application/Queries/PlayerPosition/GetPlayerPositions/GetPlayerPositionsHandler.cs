using MediatR;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Queries.PlayerPosition.GetPlayerPositions;

/// <summary>
/// Handles <see cref="GetPlayerPositionsRequest"/> queries.
/// </summary>
public class GetPlayerPositionsHandler : IRequestHandler<GetPlayerPositionsRequest, GetPlayerPositionsResponse>
{
    private readonly IPlayerPositionRepository _playerPositionRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetPlayerPositionsHandler"/> class.
    /// </summary>
    /// <param name="playerPositionRepository">The repository used to load player positions.</param>
    public GetPlayerPositionsHandler(IPlayerPositionRepository playerPositionRepository)
    {
        _playerPositionRepository = playerPositionRepository;
    }

    /// <summary>
    /// Retrieves every position rating belonging to the requested player.
    /// </summary>
    /// <param name="request">The query request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The response carrying the matching player position ratings.</returns>
    public async Task<GetPlayerPositionsResponse> Handle(GetPlayerPositionsRequest request, CancellationToken cancellationToken)
    {
        // No player-existence check here: a filter matching nothing is not an
        // error, just an empty list, and this keeps IPlayerRepository out of a
        // handler that only needs IPlayerPositionRepository.
        var playerPositions = await _playerPositionRepository.GetByPlayerIdAsync(request.PlayerId);

        return new GetPlayerPositionsResponse { Data = playerPositions.Select(GetPlayerPositionsMapper.ToDto).ToList() };
    }
}
