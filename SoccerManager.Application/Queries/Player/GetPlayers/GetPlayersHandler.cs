using MediatR;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Queries.Player.GetPlayers;

/// <summary>
/// Handles <see cref="GetPlayersRequest"/> queries.
/// </summary>
public class GetPlayersHandler : IRequestHandler<GetPlayersRequest, GetPlayersResponse>
{
    private readonly IPlayerRepository _playerRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetPlayersHandler"/> class.
    /// </summary>
    /// <param name="playerRepository">The repository used to load players.</param>
    public GetPlayersHandler(IPlayerRepository playerRepository)
    {
        _playerRepository = playerRepository;
    }

    /// <summary>
    /// Retrieves all players.
    /// </summary>
    /// <param name="request">The query request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The response carrying every player.</returns>
    public async Task<GetPlayersResponse> Handle(GetPlayersRequest request, CancellationToken cancellationToken)
    {
        var players = await _playerRepository.GetAllAsync();

        return new GetPlayersResponse { Data = players.Select(GetPlayersMapper.ToDto).ToList() };
    }
}
