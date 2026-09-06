using MediatR;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Queries.Season.GetSeasons;

/// <summary>
/// Handles <see cref="GetSeasonsRequest"/> queries.
/// </summary>
public class GetSeasonsHandler : IRequestHandler<GetSeasonsRequest, GetSeasonsResponse>
{
    private readonly ISeasonRepository _seasonRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetSeasonsHandler"/> class.
    /// </summary>
    /// <param name="seasonRepository">The repository used to load seasons.</param>
    public GetSeasonsHandler(ISeasonRepository seasonRepository)
    {
        _seasonRepository = seasonRepository;
    }

    /// <summary>
    /// Retrieves every season belonging to the requested league.
    /// </summary>
    /// <param name="request">The query request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The response carrying the matching seasons.</returns>
    public async Task<GetSeasonsResponse> Handle(GetSeasonsRequest request, CancellationToken cancellationToken)
    {
        // No league-existence check here: a filter matching nothing is not an
        // error, just an empty list, and this keeps ILeagueRepository out of a
        // handler that only needs ISeasonRepository.
        var seasons = await _seasonRepository.GetByLeagueIdAsync(request.LeagueId);

        return new GetSeasonsResponse { Data = seasons.Select(GetSeasonsMapper.ToDto).ToList() };
    }
}
