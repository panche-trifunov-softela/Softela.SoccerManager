using MediatR;
using SoccerManager.Application.Core.Exceptions;
using SoccerManager.Application.Core.PlayerSearch;
using SoccerManager.Application.Repositories;
using SoccerManager.Application.Services;

namespace SoccerManager.Application.Queries.LeagueTeam.SearchPlayersInLeague;

/// <summary>
/// Handles <see cref="SearchPlayersInLeagueRequest"/> queries.
/// </summary>
public class SearchPlayersInLeagueHandler : IRequestHandler<SearchPlayersInLeagueRequest, SearchPlayersInLeagueResponse>
{
    private readonly ILeagueTeamRepository _leagueTeamRepository;
    private readonly IClubAccess _clubAccess;

    /// <summary>
    /// Initializes a new instance of the <see cref="SearchPlayersInLeagueHandler"/> class.
    /// </summary>
    /// <param name="leagueTeamRepository">The repository used to search the league's players.</param>
    /// <param name="clubAccess">The check that the caller may manage the club.</param>
    public SearchPlayersInLeagueHandler(ILeagueTeamRepository leagueTeamRepository, IClubAccess clubAccess)
    {
        _leagueTeamRepository = leagueTeamRepository;
        _clubAccess = clubAccess;
    }

    /// <summary>
    /// Retrieves one page of the players matching the request, seen from the league of the league team, with the defaults
    /// applied for anything it leaves out. A player registered at no club of the league is a free agent.
    /// </summary>
    /// <param name="request">The query request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The response carrying the page of players, the total match count, and the page and page size applied.</returns>
    /// <exception cref="ForbiddenAccessException">Thrown when the caller is neither an operator nor the club's current manager.</exception>
    public async Task<SearchPlayersInLeagueResponse> Handle(SearchPlayersInLeagueRequest request, CancellationToken cancellationToken)
    {
        // Manage, not view: the league team anchors "your club's registration first", and the rows lead to its Bids tab.
        await _clubAccess.EnsureCanManageClubAsync(request.LeagueTeamId);

        var criteria = PlayerSearchFiltersMapper.ToCriteria(request);

        var page = await _leagueTeamRepository.SearchPlayersInLeagueAsync(request.LeagueTeamId, criteria);

        // The applied page and page size are echoed, so a caller that sent neither can still count the pages.
        return new SearchPlayersInLeagueResponse
        {
            Data = SearchPlayersInLeagueMapper.ToDto(page, criteria),
        };
    }
}
