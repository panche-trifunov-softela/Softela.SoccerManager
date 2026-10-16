using MediatR;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Queries.PlayerRating.GetPlayerRatings;

/// <summary>
/// Handles <see cref="GetPlayerRatingsRequest"/> queries.
/// </summary>
public class GetPlayerRatingsHandler : IRequestHandler<GetPlayerRatingsRequest, GetPlayerRatingsResponse>
{
    private readonly IPlayerRatingRepository _playerRatingRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetPlayerRatingsHandler"/> class.
    /// </summary>
    /// <param name="playerRatingRepository">The repository used to load player ratings.</param>
    public GetPlayerRatingsHandler(IPlayerRatingRepository playerRatingRepository)
    {
        _playerRatingRepository = playerRatingRepository;
    }

    /// <summary>
    /// Retrieves every rating the requested player has had, newest first.
    /// </summary>
    /// <param name="request">The query request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The response carrying the matching player ratings.</returns>
    public async Task<GetPlayerRatingsResponse> Handle(GetPlayerRatingsRequest request, CancellationToken cancellationToken)
    {
        // No player-existence check here: a filter matching nothing is not an
        // error, just an empty list, and this keeps IPlayerRepository out of a
        // handler that only needs IPlayerRatingRepository.
        var playerRatings = await _playerRatingRepository.GetByPlayerIdAsync(request.PlayerId);

        return new GetPlayerRatingsResponse { Data = playerRatings.Select(GetPlayerRatingsMapper.ToDto).ToList() };
    }
}
