using MediatR;
using SoccerManager.Application.Core.Exceptions;
using SoccerManager.Application.Dtos.LeagueSearch;
using SoccerManager.Application.Repositories;
using SoccerManager.Application.Services;

namespace SoccerManager.Application.Queries.LeagueTeam.SearchLeaguePlayersAndTeams;

/// <summary>
/// Handles <see cref="SearchLeaguePlayersAndTeamsRequest"/> queries.
/// </summary>
public class SearchLeaguePlayersAndTeamsHandler : IRequestHandler<SearchLeaguePlayersAndTeamsRequest, SearchLeaguePlayersAndTeamsResponse>
{
    /// <summary>
    /// The most players, and the most clubs, a search returns.
    /// </summary>
    private const int ResultsPerGroup = 5;

    private readonly ILeagueTeamRepository _leagueTeamRepository;
    private readonly IClubAccess _clubAccess;

    /// <summary>
    /// Initializes a new instance of the <see cref="SearchLeaguePlayersAndTeamsHandler"/> class.
    /// </summary>
    /// <param name="leagueTeamRepository">The repository used to search the league's players and clubs.</param>
    /// <param name="clubAccess">The check that the caller may manage the club.</param>
    public SearchLeaguePlayersAndTeamsHandler(ILeagueTeamRepository leagueTeamRepository, IClubAccess clubAccess)
    {
        _leagueTeamRepository = leagueTeamRepository;
        _clubAccess = clubAccess;
    }

    /// <summary>
    /// Searches the players and the clubs of the league of the league team for the requested text, and returns the best
    /// few of each with how many match in all. A player registered at no club of the league is a free agent.
    /// </summary>
    /// <param name="request">The query request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The response carrying the best matching players and clubs, each group with its total match count.</returns>
    /// <exception cref="ForbiddenAccessException">Thrown when the caller is neither an operator nor the club's current manager.</exception>
    public async Task<SearchLeaguePlayersAndTeamsResponse> Handle(SearchLeaguePlayersAndTeamsRequest request, CancellationToken cancellationToken)
    {
        // There is no existence check: an unknown league team has no current manager, so it is a 403 for anyone but an
        // operator, who gets two empty groups.
        await _clubAccess.EnsureCanManageClubAsync(request.LeagueTeamId);

        var searchTerm = SearchLeaguePlayersAndTeamsMapper.ToSearchTerm(request.SearchTerm);

        var (playerTotalCount, players, teamTotalCount, teams) = await _leagueTeamRepository.SearchLeaguePlayersAndTeamsAsync(request.LeagueTeamId, searchTerm, ResultsPerGroup);

        return new SearchLeaguePlayersAndTeamsResponse
        {
            Data = new LeagueSearchResultDto
            {
                Players = new LeagueSearchPlayerGroupDto
                {
                    Items = players.Select(SearchLeaguePlayersAndTeamsMapper.ToPlayerDto).ToList(),
                    TotalCount = playerTotalCount,
                },
                Teams = new LeagueSearchTeamGroupDto
                {
                    Items = teams.Select(SearchLeaguePlayersAndTeamsMapper.ToTeamDto).ToList(),
                    TotalCount = teamTotalCount,
                },
            },
        };
    }
}
