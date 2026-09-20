using MediatR;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Queries.MatchTeamTactic.GetMatchTeamTacticById;

/// <summary>
/// Handles <see cref="GetMatchTeamTacticByIdRequest"/> queries.
/// </summary>
public class GetMatchTeamTacticByIdHandler : IRequestHandler<GetMatchTeamTacticByIdRequest, GetMatchTeamTacticByIdResponse>
{
    private readonly IMatchTeamTacticRepository _matchTeamTacticRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetMatchTeamTacticByIdHandler"/> class.
    /// </summary>
    /// <param name="matchTeamTacticRepository">The repository used to load match team tactics.</param>
    public GetMatchTeamTacticByIdHandler(IMatchTeamTacticRepository matchTeamTacticRepository)
    {
        _matchTeamTacticRepository = matchTeamTacticRepository;
    }

    /// <summary>
    /// Retrieves the match team tactic identified by the given request.
    /// </summary>
    /// <param name="request">The query request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The response carrying the requested match team tactic.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when no match team tactic with the given identifier exists.</exception>
    public async Task<GetMatchTeamTacticByIdResponse> Handle(GetMatchTeamTacticByIdRequest request, CancellationToken cancellationToken)
    {
        var matchTeamTactic = await _matchTeamTacticRepository.GetByIdAsync(request.Id)
            ?? throw new KeyNotFoundException($"Match team tactic {request.Id} not found.");

        return new GetMatchTeamTacticByIdResponse { Data = GetMatchTeamTacticByIdMapper.ToDto(matchTeamTactic) };
    }
}
