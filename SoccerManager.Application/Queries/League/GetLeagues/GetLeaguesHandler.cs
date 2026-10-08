using MediatR;
using SoccerManager.Application.Core.User;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Queries.League.GetLeagues;

/// <summary>
/// Handles <see cref="GetLeaguesRequest"/> queries.
/// </summary>
public class GetLeaguesHandler : IRequestHandler<GetLeaguesRequest, GetLeaguesResponse>
{
    /// <summary>
    /// The most leagues a single request returns, with or without a search term.
    /// </summary>
    private const int MaxLeagueCount = 10;

    private readonly ILeagueRepository _leagueRepository;
    private readonly ICurrentUser _currentUser;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetLeaguesHandler"/> class.
    /// </summary>
    /// <param name="leagueRepository">The repository used to load leagues.</param>
    /// <param name="currentUser">The currently authenticated user, whose managed leagues are left out.</param>
    public GetLeaguesHandler(ILeagueRepository leagueRepository, ICurrentUser currentUser)
    {
        _leagueRepository = leagueRepository;
        _currentUser = currentUser;
    }

    /// <summary>
    /// Retrieves the newest leagues in which the current user does not currently manage a club, optionally filtered by name.
    /// </summary>
    /// <param name="request">The query request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The response carrying the available leagues, newest first.</returns>
    public async Task<GetLeaguesResponse> Handle(GetLeaguesRequest request, CancellationToken cancellationToken)
    {
        // A blank term means no name filter. It must not reach the procedure as an
        // empty string, which CHARINDEX would match against nothing.
        var searchTerm = string.IsNullOrWhiteSpace(request.SearchTerm) ? null : request.SearchTerm.Trim();

        var leagues = await _leagueRepository.GetAvailableForUserAsync(_currentUser.UserId, searchTerm, MaxLeagueCount);

        return new GetLeaguesResponse { Data = leagues.Select(GetLeaguesMapper.ToDto).ToList() };
    }
}
