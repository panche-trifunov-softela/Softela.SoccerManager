using MediatR;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Queries.Standing.GetStandings;

/// <summary>
/// Handles <see cref="GetStandingsRequest"/> queries.
/// </summary>
public class GetStandingsHandler : IRequestHandler<GetStandingsRequest, GetStandingsResponse>
{
    private readonly IStandingRepository _standingRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetStandingsHandler"/> class.
    /// </summary>
    /// <param name="standingRepository">The repository used to load standings.</param>
    public GetStandingsHandler(IStandingRepository standingRepository)
    {
        _standingRepository = standingRepository;
    }

    /// <summary>
    /// Retrieves every standing belonging to the requested season and division.
    /// </summary>
    /// <param name="request">The query request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The response carrying the matching standings.</returns>
    public async Task<GetStandingsResponse> Handle(GetStandingsRequest request, CancellationToken cancellationToken)
    {
        var standings = await _standingRepository.GetBySeasonAndDivisionAsync(request.SeasonId, request.DivisionId);

        return new GetStandingsResponse { Data = standings.Select(GetStandingsMapper.ToDto).ToList() };
    }
}
