using MediatR;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Queries.Competition.GetCompetitions;

/// <summary>
/// Handles <see cref="GetCompetitionsRequest"/> queries.
/// </summary>
public class GetCompetitionsHandler : IRequestHandler<GetCompetitionsRequest, GetCompetitionsResponse>
{
    private readonly ICompetitionRepository _competitionRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetCompetitionsHandler"/> class.
    /// </summary>
    /// <param name="competitionRepository">The repository used to load competitions.</param>
    public GetCompetitionsHandler(ICompetitionRepository competitionRepository)
    {
        _competitionRepository = competitionRepository;
    }

    /// <summary>
    /// Retrieves every competition belonging to the requested league.
    /// </summary>
    /// <param name="request">The query request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The response carrying the matching competitions.</returns>
    public async Task<GetCompetitionsResponse> Handle(GetCompetitionsRequest request, CancellationToken cancellationToken)
    {
        // No league-existence check here: a filter matching nothing is not an
        // error, just an empty list, and this keeps ILeagueRepository out of a
        // handler that only needs ICompetitionRepository.
        var competitions = await _competitionRepository.GetByLeagueIdAsync(request.LeagueId);

        return new GetCompetitionsResponse { Data = competitions.Select(GetCompetitionsMapper.ToDto).ToList() };
    }
}
