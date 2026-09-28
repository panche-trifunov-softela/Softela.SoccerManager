using MediatR;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Queries.NationalTeam.GetNationalTeams;

/// <summary>
/// Handles <see cref="GetNationalTeamsRequest"/> queries.
/// </summary>
public class GetNationalTeamsHandler : IRequestHandler<GetNationalTeamsRequest, GetNationalTeamsResponse>
{
    private readonly INationalTeamRepository _nationalTeamRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetNationalTeamsHandler"/> class.
    /// </summary>
    /// <param name="nationalTeamRepository">The repository used to load national teams.</param>
    public GetNationalTeamsHandler(INationalTeamRepository nationalTeamRepository)
    {
        _nationalTeamRepository = nationalTeamRepository;
    }

    /// <summary>
    /// Retrieves all national teams.
    /// </summary>
    /// <param name="request">The query request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The response carrying every national team.</returns>
    public async Task<GetNationalTeamsResponse> Handle(GetNationalTeamsRequest request, CancellationToken cancellationToken)
    {
        var nationalTeams = await _nationalTeamRepository.GetAllAsync();

        return new GetNationalTeamsResponse { Data = nationalTeams.Select(GetNationalTeamsMapper.ToDto).ToList() };
    }
}
