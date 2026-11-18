using MediatR;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Queries.NationalTeamMatch.GetNationalTeamMatches;

/// <summary>
/// Handles <see cref="GetNationalTeamMatchesRequest"/> queries.
/// </summary>
public class GetNationalTeamMatchesHandler : IRequestHandler<GetNationalTeamMatchesRequest, GetNationalTeamMatchesResponse>
{
    private readonly INationalTeamMatchRepository _nationalTeamMatchRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetNationalTeamMatchesHandler"/> class.
    /// </summary>
    /// <param name="nationalTeamMatchRepository">The repository used to load national team matches.</param>
    public GetNationalTeamMatchesHandler(INationalTeamMatchRepository nationalTeamMatchRepository)
    {
        _nationalTeamMatchRepository = nationalTeamMatchRepository;
    }

    /// <summary>
    /// Retrieves every national team match of the requested season, in kickoff order.
    /// </summary>
    /// <param name="request">The query request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The response carrying the matching national team matches.</returns>
    public async Task<GetNationalTeamMatchesResponse> Handle(GetNationalTeamMatchesRequest request, CancellationToken cancellationToken)
    {
        // No season-existence check here: a filter matching nothing is not an
        // error, just an empty list, and this keeps ISeasonRepository out of a
        // handler that only needs INationalTeamMatchRepository.
        var nationalTeamMatches = await _nationalTeamMatchRepository.GetBySeasonIdAsync(request.SeasonId);

        return new GetNationalTeamMatchesResponse { Data = nationalTeamMatches.Select(GetNationalTeamMatchesMapper.ToDto).ToList() };
    }
}
