using MediatR;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Queries.LeagueTeamPlayerInjury.GetLeagueTeamPlayerInjuryById;

/// <summary>
/// Handles <see cref="GetLeagueTeamPlayerInjuryByIdRequest"/> queries.
/// </summary>
public class GetLeagueTeamPlayerInjuryByIdHandler : IRequestHandler<GetLeagueTeamPlayerInjuryByIdRequest, GetLeagueTeamPlayerInjuryByIdResponse>
{
    private readonly ILeagueTeamPlayerInjuryRepository _leagueTeamPlayerInjuryRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetLeagueTeamPlayerInjuryByIdHandler"/> class.
    /// </summary>
    /// <param name="leagueTeamPlayerInjuryRepository">The repository used to load injuries.</param>
    public GetLeagueTeamPlayerInjuryByIdHandler(ILeagueTeamPlayerInjuryRepository leagueTeamPlayerInjuryRepository)
    {
        _leagueTeamPlayerInjuryRepository = leagueTeamPlayerInjuryRepository;
    }

    /// <summary>
    /// Retrieves the injury identified by the given request.
    /// </summary>
    /// <param name="request">The query request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The response carrying the requested injury.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when no injury with the given identifier exists.</exception>
    public async Task<GetLeagueTeamPlayerInjuryByIdResponse> Handle(GetLeagueTeamPlayerInjuryByIdRequest request, CancellationToken cancellationToken)
    {
        var leagueTeamPlayerInjury = await _leagueTeamPlayerInjuryRepository.GetByIdAsync(request.Id)
            ?? throw new KeyNotFoundException($"League team player injury {request.Id} not found.");

        return new GetLeagueTeamPlayerInjuryByIdResponse { Data = GetLeagueTeamPlayerInjuryByIdMapper.ToDto(leagueTeamPlayerInjury) };
    }
}
