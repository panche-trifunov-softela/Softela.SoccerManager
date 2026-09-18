using MediatR;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Queries.MatchTeamStatistic.GetMatchTeamStatisticById;

/// <summary>
/// Handles <see cref="GetMatchTeamStatisticByIdRequest"/> queries.
/// </summary>
public class GetMatchTeamStatisticByIdHandler : IRequestHandler<GetMatchTeamStatisticByIdRequest, GetMatchTeamStatisticByIdResponse>
{
    private readonly IMatchTeamStatisticRepository _matchTeamStatisticRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetMatchTeamStatisticByIdHandler"/> class.
    /// </summary>
    /// <param name="matchTeamStatisticRepository">The repository used to load match team statistics.</param>
    public GetMatchTeamStatisticByIdHandler(IMatchTeamStatisticRepository matchTeamStatisticRepository)
    {
        _matchTeamStatisticRepository = matchTeamStatisticRepository;
    }

    /// <summary>
    /// Retrieves the match team statistic identified by the given request.
    /// </summary>
    /// <param name="request">The query request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The response carrying the requested match team statistic.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when no match team statistic with the given identifier exists.</exception>
    public async Task<GetMatchTeamStatisticByIdResponse> Handle(GetMatchTeamStatisticByIdRequest request, CancellationToken cancellationToken)
    {
        var matchTeamStatistic = await _matchTeamStatisticRepository.GetByIdAsync(request.Id)
            ?? throw new KeyNotFoundException($"Match team statistic {request.Id} not found.");

        return new GetMatchTeamStatisticByIdResponse { Data = GetMatchTeamStatisticByIdMapper.ToDto(matchTeamStatistic) };
    }
}
