using MediatR;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Queries.MatchPlayerStatistic.GetMatchPlayerStatisticById;

/// <summary>
/// Handles <see cref="GetMatchPlayerStatisticByIdRequest"/> queries.
/// </summary>
public class GetMatchPlayerStatisticByIdHandler : IRequestHandler<GetMatchPlayerStatisticByIdRequest, GetMatchPlayerStatisticByIdResponse>
{
    private readonly IMatchPlayerStatisticRepository _matchPlayerStatisticRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetMatchPlayerStatisticByIdHandler"/> class.
    /// </summary>
    /// <param name="matchPlayerStatisticRepository">The repository used to load match player statistics.</param>
    public GetMatchPlayerStatisticByIdHandler(IMatchPlayerStatisticRepository matchPlayerStatisticRepository)
    {
        _matchPlayerStatisticRepository = matchPlayerStatisticRepository;
    }

    /// <summary>
    /// Retrieves the match player statistic identified by the given request.
    /// </summary>
    /// <param name="request">The query request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The response carrying the requested match player statistic.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when no match player statistic with the given identifier exists.</exception>
    public async Task<GetMatchPlayerStatisticByIdResponse> Handle(GetMatchPlayerStatisticByIdRequest request, CancellationToken cancellationToken)
    {
        var matchPlayerStatistic = await _matchPlayerStatisticRepository.GetByIdAsync(request.Id)
            ?? throw new KeyNotFoundException($"Match player statistic {request.Id} not found.");

        return new GetMatchPlayerStatisticByIdResponse { Data = GetMatchPlayerStatisticByIdMapper.ToDto(matchPlayerStatistic) };
    }
}
