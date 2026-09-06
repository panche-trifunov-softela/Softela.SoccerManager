using MediatR;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Queries.League.GetLeagues;

/// <summary>
/// Handles <see cref="GetLeaguesRequest"/> queries.
/// </summary>
public class GetLeaguesHandler : IRequestHandler<GetLeaguesRequest, GetLeaguesResponse>
{
    private readonly ILeagueRepository _leagueRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetLeaguesHandler"/> class.
    /// </summary>
    /// <param name="leagueRepository">The repository used to load leagues.</param>
    public GetLeaguesHandler(ILeagueRepository leagueRepository)
    {
        _leagueRepository = leagueRepository;
    }

    /// <summary>
    /// Retrieves all leagues.
    /// </summary>
    /// <param name="request">The query request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The response carrying every league.</returns>
    public async Task<GetLeaguesResponse> Handle(GetLeaguesRequest request, CancellationToken cancellationToken)
    {
        var leagues = await _leagueRepository.GetAllAsync();

        return new GetLeaguesResponse { Data = leagues.Select(GetLeaguesMapper.ToDto).ToList() };
    }
}
