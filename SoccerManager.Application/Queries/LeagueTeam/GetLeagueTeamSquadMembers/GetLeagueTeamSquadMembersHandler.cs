using MediatR;
using SoccerManager.Application.Core.Exceptions;
using SoccerManager.Application.Repositories;
using SoccerManager.Application.Services;

namespace SoccerManager.Application.Queries.LeagueTeam.GetLeagueTeamSquadMembers;

/// <summary>
/// Handles <see cref="GetLeagueTeamSquadMembersRequest"/> queries.
/// </summary>
public class GetLeagueTeamSquadMembersHandler : IRequestHandler<GetLeagueTeamSquadMembersRequest, GetLeagueTeamSquadMembersResponse>
{
    private readonly ILeagueTeamRepository _leagueTeamRepository;
    private readonly IClubAccess _clubAccess;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetLeagueTeamSquadMembersHandler"/> class.
    /// </summary>
    /// <param name="leagueTeamRepository">The repository used to load the league team's squad.</param>
    /// <param name="clubAccess">The check that the caller may manage the club.</param>
    public GetLeagueTeamSquadMembersHandler(ILeagueTeamRepository leagueTeamRepository, IClubAccess clubAccess)
    {
        _leagueTeamRepository = leagueTeamRepository;
        _clubAccess = clubAccess;
    }

    /// <summary>
    /// Retrieves the requested league team's squad entries, each with the player's date of birth, current rating, main
    /// position and national team, and the entry's squad number, value, weekly wage, morale and condition.
    /// </summary>
    /// <param name="request">The query request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The response carrying the squad entries, ordered by line and position.</returns>
    /// <exception cref="ForbiddenAccessException">Thrown when the caller is neither an operator nor the club's current manager.</exception>
    public async Task<GetLeagueTeamSquadMembersResponse> Handle(GetLeagueTeamSquadMembersRequest request, CancellationToken cancellationToken)
    {
        // There is no existence check: an unknown league team has no current manager, so it is a 403 for anyone but an
        // operator, who gets an empty list.
        await _clubAccess.EnsureCanManageClubAsync(request.LeagueTeamId);

        var rows = await _leagueTeamRepository.GetSquadMembersAsync(request.LeagueTeamId);

        return new GetLeagueTeamSquadMembersResponse { Data = rows.Select(GetLeagueTeamSquadMembersMapper.ToDto).ToList() };
    }
}
