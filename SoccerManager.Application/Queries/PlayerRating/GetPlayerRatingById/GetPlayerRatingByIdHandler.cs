using MediatR;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Queries.PlayerRating.GetPlayerRatingById;

/// <summary>
/// Handles <see cref="GetPlayerRatingByIdRequest"/> queries.
/// </summary>
public class GetPlayerRatingByIdHandler : IRequestHandler<GetPlayerRatingByIdRequest, GetPlayerRatingByIdResponse>
{
    private readonly IPlayerRatingRepository _playerRatingRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetPlayerRatingByIdHandler"/> class.
    /// </summary>
    /// <param name="playerRatingRepository">The repository used to load player ratings.</param>
    public GetPlayerRatingByIdHandler(IPlayerRatingRepository playerRatingRepository)
    {
        _playerRatingRepository = playerRatingRepository;
    }

    /// <summary>
    /// Retrieves the player rating identified by the given request.
    /// </summary>
    /// <param name="request">The query request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The response carrying the requested player rating.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when no player rating with the given identifier exists.</exception>
    public async Task<GetPlayerRatingByIdResponse> Handle(GetPlayerRatingByIdRequest request, CancellationToken cancellationToken)
    {
        var playerRating = await _playerRatingRepository.GetByIdAsync(request.Id)
            ?? throw new KeyNotFoundException($"Player rating {request.Id} not found.");

        return new GetPlayerRatingByIdResponse { Data = GetPlayerRatingByIdMapper.ToDto(playerRating) };
    }
}
