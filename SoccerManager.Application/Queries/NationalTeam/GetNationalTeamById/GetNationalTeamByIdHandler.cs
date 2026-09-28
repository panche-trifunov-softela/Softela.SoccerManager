using MediatR;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Queries.NationalTeam.GetNationalTeamById;

/// <summary>
/// Handles <see cref="GetNationalTeamByIdRequest"/> queries.
/// </summary>
public class GetNationalTeamByIdHandler : IRequestHandler<GetNationalTeamByIdRequest, GetNationalTeamByIdResponse>
{
    private readonly INationalTeamRepository _nationalTeamRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetNationalTeamByIdHandler"/> class.
    /// </summary>
    /// <param name="nationalTeamRepository">The repository used to load national teams.</param>
    public GetNationalTeamByIdHandler(INationalTeamRepository nationalTeamRepository)
    {
        _nationalTeamRepository = nationalTeamRepository;
    }

    /// <summary>
    /// Retrieves the national team identified by the given request.
    /// </summary>
    /// <param name="request">The query request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The response carrying the requested national team.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when no national team with the given identifier exists.</exception>
    public async Task<GetNationalTeamByIdResponse> Handle(GetNationalTeamByIdRequest request, CancellationToken cancellationToken)
    {
        var nationalTeam = await _nationalTeamRepository.GetByIdAsync(request.Id)
            ?? throw new KeyNotFoundException($"NationalTeam {request.Id} not found.");

        return new GetNationalTeamByIdResponse { Data = GetNationalTeamByIdMapper.ToDto(nationalTeam) };
    }
}
