using MediatR;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Queries.Player.GetPlayerProfile;

/// <summary>
/// Handles <see cref="GetPlayerProfileRequest"/> queries.
/// </summary>
public class GetPlayerProfileHandler : IRequestHandler<GetPlayerProfileRequest, GetPlayerProfileResponse>
{
    private readonly IPlayerRepository _playerRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetPlayerProfileHandler"/> class.
    /// </summary>
    /// <param name="playerRepository">The repository used to load players.</param>
    public GetPlayerProfileHandler(IPlayerRepository playerRepository)
    {
        _playerRepository = playerRepository;
    }

    /// <summary>
    /// Retrieves the profile of the player identified by the given request: the player with the real-life club, national
    /// team, main position, current rating, value and wage, and every position the player can play.
    /// </summary>
    /// <param name="request">The query request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The response carrying the requested player's profile.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when no player with the given identifier exists.</exception>
    public async Task<GetPlayerProfileResponse> Handle(GetPlayerProfileRequest request, CancellationToken cancellationToken)
    {
        var (header, positions) = await _playerRepository.GetProfileAsync(request.Id);

        if (header is null)
        {
            throw new KeyNotFoundException($"Player {request.Id} not found.");
        }

        return new GetPlayerProfileResponse { Data = GetPlayerProfileMapper.ToDto(header, positions) };
    }
}
