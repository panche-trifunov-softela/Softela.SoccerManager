using MediatR;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Queries.Stadium.GetStadiums;

/// <summary>
/// Handles <see cref="GetStadiumsRequest"/> queries.
/// </summary>
public class GetStadiumsHandler : IRequestHandler<GetStadiumsRequest, GetStadiumsResponse>
{
    private readonly IStadiumRepository _stadiumRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetStadiumsHandler"/> class.
    /// </summary>
    /// <param name="stadiumRepository">The repository used to load stadiums.</param>
    public GetStadiumsHandler(IStadiumRepository stadiumRepository)
    {
        _stadiumRepository = stadiumRepository;
    }

    /// <summary>
    /// Retrieves all stadiums.
    /// </summary>
    /// <param name="request">The query request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The response carrying every stadium.</returns>
    public async Task<GetStadiumsResponse> Handle(GetStadiumsRequest request, CancellationToken cancellationToken)
    {
        var stadiums = await _stadiumRepository.GetAllAsync();

        return new GetStadiumsResponse { Data = stadiums.Select(GetStadiumsMapper.ToDto).ToList() };
    }
}
