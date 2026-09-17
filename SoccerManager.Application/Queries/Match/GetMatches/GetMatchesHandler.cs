using MediatR;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Queries.Match.GetMatches;

/// <summary>
/// Handles <see cref="GetMatchesRequest"/> queries.
/// </summary>
public class GetMatchesHandler : IRequestHandler<GetMatchesRequest, GetMatchesResponse>
{
    private readonly IMatchRepository _matchRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetMatchesHandler"/> class.
    /// </summary>
    /// <param name="matchRepository">The repository used to load matches.</param>
    public GetMatchesHandler(IMatchRepository matchRepository)
    {
        _matchRepository = matchRepository;
    }

    /// <summary>
    /// Retrieves all matches.
    /// </summary>
    /// <param name="request">The query request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The response carrying every match.</returns>
    public async Task<GetMatchesResponse> Handle(GetMatchesRequest request, CancellationToken cancellationToken)
    {
        var matches = await _matchRepository.GetAllAsync();

        return new GetMatchesResponse { Data = matches.Select(GetMatchesMapper.ToDto).ToList() };
    }
}
