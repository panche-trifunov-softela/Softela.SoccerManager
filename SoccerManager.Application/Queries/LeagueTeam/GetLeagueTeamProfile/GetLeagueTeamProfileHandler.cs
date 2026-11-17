using MediatR;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Queries.LeagueTeam.GetLeagueTeamProfile;

/// <summary>
/// Handles <see cref="GetLeagueTeamProfileRequest"/> queries.
/// </summary>
public class GetLeagueTeamProfileHandler : IRequestHandler<GetLeagueTeamProfileRequest, GetLeagueTeamProfileResponse>
{
    private readonly ILeagueTeamRepository _leagueTeamRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetLeagueTeamProfileHandler"/> class.
    /// </summary>
    /// <param name="leagueTeamRepository">The repository used to load the league team's profile.</param>
    public GetLeagueTeamProfileHandler(ILeagueTeamRepository leagueTeamRepository)
    {
        _leagueTeamRepository = leagueTeamRepository;
    }

    /// <summary>
    /// Retrieves the profile of the league team identified by the given request.
    /// </summary>
    /// <param name="request">The query request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The response carrying the league team's profile.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when no league team with the given identifier exists.</exception>
    public async Task<GetLeagueTeamProfileResponse> Handle(GetLeagueTeamProfileRequest request, CancellationToken cancellationToken)
    {
        // There is no access check: a club's team name, logo and league are public to every signed-in user, as the league and
        // team names already are, so any caller may read any club's profile.
        var row = await _leagueTeamRepository.GetProfileAsync(request.LeagueTeamId)
            ?? throw new KeyNotFoundException($"LeagueTeam {request.LeagueTeamId} not found.");

        return new GetLeagueTeamProfileResponse { Data = GetLeagueTeamProfileMapper.ToDto(row) };
    }
}
