using MediatR;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Queries.MatchTeamTactic.GetMatchTeamTactics;

/// <summary>
/// Handles <see cref="GetMatchTeamTacticsRequest"/> queries.
/// </summary>
public class GetMatchTeamTacticsHandler : IRequestHandler<GetMatchTeamTacticsRequest, GetMatchTeamTacticsResponse>
{
    private readonly IMatchTeamTacticRepository _matchTeamTacticRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetMatchTeamTacticsHandler"/> class.
    /// </summary>
    /// <param name="matchTeamTacticRepository">The repository used to load match team tactics.</param>
    public GetMatchTeamTacticsHandler(IMatchTeamTacticRepository matchTeamTacticRepository)
    {
        _matchTeamTacticRepository = matchTeamTacticRepository;
    }

    /// <summary>
    /// Retrieves every team tactic recorded for the requested match.
    /// </summary>
    /// <param name="request">The query request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The response carrying the matching team tactics.</returns>
    public async Task<GetMatchTeamTacticsResponse> Handle(GetMatchTeamTacticsRequest request, CancellationToken cancellationToken)
    {
        // No match-existence check here: a filter matching nothing is not an
        // error, just an empty list, and this keeps IMatchRepository out of a
        // handler that only needs IMatchTeamTacticRepository.
        var matchTeamTactics = await _matchTeamTacticRepository.GetByMatchIdAsync(request.MatchId);

        return new GetMatchTeamTacticsResponse { Data = matchTeamTactics.Select(GetMatchTeamTacticsMapper.ToDto).ToList() };
    }
}
