using MediatR;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Queries.Season.GetSeasonById;

/// <summary>
/// Handles <see cref="GetSeasonByIdRequest"/> queries.
/// </summary>
public class GetSeasonByIdHandler : IRequestHandler<GetSeasonByIdRequest, GetSeasonByIdResponse>
{
    private readonly ISeasonRepository _seasonRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetSeasonByIdHandler"/> class.
    /// </summary>
    /// <param name="seasonRepository">The repository used to load seasons.</param>
    public GetSeasonByIdHandler(ISeasonRepository seasonRepository)
    {
        _seasonRepository = seasonRepository;
    }

    /// <summary>
    /// Retrieves the season identified by the given request.
    /// </summary>
    /// <param name="request">The query request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The response carrying the requested season.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when no season with the given identifier exists.</exception>
    public async Task<GetSeasonByIdResponse> Handle(GetSeasonByIdRequest request, CancellationToken cancellationToken)
    {
        var season = await _seasonRepository.GetByIdAsync(request.Id)
            ?? throw new KeyNotFoundException($"Season {request.Id} not found.");

        return new GetSeasonByIdResponse { Data = GetSeasonByIdMapper.ToDto(season) };
    }
}
