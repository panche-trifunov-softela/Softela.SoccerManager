using MediatR;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Queries.PlayerPosition.GetPlayerPositionById;

/// <summary>
/// Handles <see cref="GetPlayerPositionByIdRequest"/> queries.
/// </summary>
public class GetPlayerPositionByIdHandler : IRequestHandler<GetPlayerPositionByIdRequest, GetPlayerPositionByIdResponse>
{
    private readonly IPlayerPositionRepository _playerPositionRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetPlayerPositionByIdHandler"/> class.
    /// </summary>
    /// <param name="playerPositionRepository">The repository used to load player positions.</param>
    public GetPlayerPositionByIdHandler(IPlayerPositionRepository playerPositionRepository)
    {
        _playerPositionRepository = playerPositionRepository;
    }

    /// <summary>
    /// Retrieves the player position rating identified by the given request.
    /// </summary>
    /// <param name="request">The query request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The response carrying the requested player position rating.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when no player position with the given identifier exists.</exception>
    public async Task<GetPlayerPositionByIdResponse> Handle(GetPlayerPositionByIdRequest request, CancellationToken cancellationToken)
    {
        var playerPosition = await _playerPositionRepository.GetByIdAsync(request.Id)
            ?? throw new KeyNotFoundException($"PlayerPosition {request.Id} not found.");

        return new GetPlayerPositionByIdResponse { Data = GetPlayerPositionByIdMapper.ToDto(playerPosition) };
    }
}
