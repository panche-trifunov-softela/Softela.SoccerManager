using MediatR;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Queries.Competition.GetCompetitionById;

/// <summary>
/// Handles <see cref="GetCompetitionByIdRequest"/> queries.
/// </summary>
public class GetCompetitionByIdHandler : IRequestHandler<GetCompetitionByIdRequest, GetCompetitionByIdResponse>
{
    private readonly ICompetitionRepository _competitionRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetCompetitionByIdHandler"/> class.
    /// </summary>
    /// <param name="competitionRepository">The repository used to load competitions.</param>
    public GetCompetitionByIdHandler(ICompetitionRepository competitionRepository)
    {
        _competitionRepository = competitionRepository;
    }

    /// <summary>
    /// Retrieves the competition identified by the given request.
    /// </summary>
    /// <param name="request">The query request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The response carrying the requested competition.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when no competition with the given identifier exists.</exception>
    public async Task<GetCompetitionByIdResponse> Handle(GetCompetitionByIdRequest request, CancellationToken cancellationToken)
    {
        var competition = await _competitionRepository.GetByIdAsync(request.Id)
            ?? throw new KeyNotFoundException($"Competition {request.Id} not found.");

        return new GetCompetitionByIdResponse { Data = GetCompetitionByIdMapper.ToDto(competition) };
    }
}
