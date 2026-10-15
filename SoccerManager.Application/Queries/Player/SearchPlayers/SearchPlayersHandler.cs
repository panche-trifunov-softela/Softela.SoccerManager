using MediatR;
using SoccerManager.Application.Dtos.PlayerSearch;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Queries.Player.SearchPlayers;

/// <summary>
/// Handles <see cref="SearchPlayersRequest"/> queries.
/// </summary>
public class SearchPlayersHandler : IRequestHandler<SearchPlayersRequest, SearchPlayersResponse>
{
    private readonly IPlayerRepository _playerRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="SearchPlayersHandler"/> class.
    /// </summary>
    /// <param name="playerRepository">The repository used to search players.</param>
    public SearchPlayersHandler(IPlayerRepository playerRepository)
    {
        _playerRepository = playerRepository;
    }

    /// <summary>
    /// Retrieves one page of the players matching the request, with the defaults applied for anything it leaves out.
    /// </summary>
    /// <param name="request">The query request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The response carrying the page of players, the total match count, and the page and page size applied.</returns>
    public async Task<SearchPlayersResponse> Handle(SearchPlayersRequest request, CancellationToken cancellationToken)
    {
        var criteria = SearchPlayersMapper.ToCriteria(request);

        var result = await _playerRepository.SearchAsync(criteria);

        // The applied page and page size are echoed, so a caller that sent neither can still count the pages.
        return new SearchPlayersResponse
        {
            Data = new PlayerSearchResultDto
            {
                Items = result.Rows.Select(SearchPlayersMapper.ToDto).ToList(),
                TotalCount = result.TotalCount,
                Page = criteria.Page,
                PageSize = criteria.PageSize,
            },
        };
    }
}
