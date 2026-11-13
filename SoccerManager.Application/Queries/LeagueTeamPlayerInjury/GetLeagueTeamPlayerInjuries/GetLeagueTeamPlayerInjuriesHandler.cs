using MediatR;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Queries.LeagueTeamPlayerInjury.GetLeagueTeamPlayerInjuries;

/// <summary>
/// Handles <see cref="GetLeagueTeamPlayerInjuriesRequest"/> queries.
/// </summary>
public class GetLeagueTeamPlayerInjuriesHandler : IRequestHandler<GetLeagueTeamPlayerInjuriesRequest, GetLeagueTeamPlayerInjuriesResponse>
{
    private readonly ILeagueTeamPlayerInjuryRepository _leagueTeamPlayerInjuryRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetLeagueTeamPlayerInjuriesHandler"/> class.
    /// </summary>
    /// <param name="leagueTeamPlayerInjuryRepository">The repository used to load injuries.</param>
    public GetLeagueTeamPlayerInjuriesHandler(ILeagueTeamPlayerInjuryRepository leagueTeamPlayerInjuryRepository)
    {
        _leagueTeamPlayerInjuryRepository = leagueTeamPlayerInjuryRepository;
    }

    /// <summary>
    /// Retrieves every injury of the requested squad row.
    /// </summary>
    /// <param name="request">The query request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The response carrying the matching injuries.</returns>
    public async Task<GetLeagueTeamPlayerInjuriesResponse> Handle(GetLeagueTeamPlayerInjuriesRequest request, CancellationToken cancellationToken)
    {
        // No squad-row-existence check here: a filter matching nothing is not an
        // error, just an empty list, and this keeps ILeagueTeamPlayerRepository out of a
        // handler that only needs ILeagueTeamPlayerInjuryRepository.
        var leagueTeamPlayerInjuries = await _leagueTeamPlayerInjuryRepository.GetByLeagueTeamPlayerIdAsync(request.LeagueTeamPlayerId);

        return new GetLeagueTeamPlayerInjuriesResponse { Data = leagueTeamPlayerInjuries.Select(GetLeagueTeamPlayerInjuriesMapper.ToDto).ToList() };
    }
}
