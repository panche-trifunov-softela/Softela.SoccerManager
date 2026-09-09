using MediatR;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Queries.Player.GetPlayerById;

/// <summary>
/// Handles <see cref="GetPlayerByIdRequest"/> queries.
/// </summary>
public class GetPlayerByIdHandler : IRequestHandler<GetPlayerByIdRequest, GetPlayerByIdResponse>
{
    private readonly IPlayerRepository _playerRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetPlayerByIdHandler"/> class.
    /// </summary>
    /// <param name="playerRepository">The repository used to load players.</param>
    public GetPlayerByIdHandler(IPlayerRepository playerRepository)
    {
        _playerRepository = playerRepository;
    }

    /// <summary>
    /// Retrieves the player identified by the given request.
    /// </summary>
    /// <param name="request">The query request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The response carrying the requested player.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when no player with the given identifier exists.</exception>
    public async Task<GetPlayerByIdResponse> Handle(GetPlayerByIdRequest request, CancellationToken cancellationToken)
    {
        var player = await _playerRepository.GetByIdAsync(request.Id)
            ?? throw new KeyNotFoundException($"Player {request.Id} not found.");

        return new GetPlayerByIdResponse { Data = GetPlayerByIdMapper.ToDto(player) };
    }
}
