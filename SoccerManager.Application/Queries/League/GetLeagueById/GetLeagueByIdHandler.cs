using MediatR;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Queries.League.GetLeagueById;

/// <summary>
/// Handles <see cref="GetLeagueByIdRequest"/> queries.
/// </summary>
public class GetLeagueByIdHandler : IRequestHandler<GetLeagueByIdRequest, GetLeagueByIdResponse>
{
    private readonly ILeagueRepository _leagueRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetLeagueByIdHandler"/> class.
    /// </summary>
    /// <param name="leagueRepository">The repository used to load leagues.</param>
    public GetLeagueByIdHandler(ILeagueRepository leagueRepository)
    {
        _leagueRepository = leagueRepository;
    }

    /// <summary>
    /// Retrieves the league identified by the given request.
    /// </summary>
    /// <param name="request">The query request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The response carrying the requested league.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when no league with the given identifier exists.</exception>
    public async Task<GetLeagueByIdResponse> Handle(GetLeagueByIdRequest request, CancellationToken cancellationToken)
    {
        var league = await _leagueRepository.GetByIdAsync(request.Id)
            ?? throw new KeyNotFoundException($"League {request.Id} not found.");

        return new GetLeagueByIdResponse { Data = GetLeagueByIdMapper.ToDto(league) };
    }
}
