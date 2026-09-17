using MediatR;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Queries.Stadium.GetStadiumById;

/// <summary>
/// Handles <see cref="GetStadiumByIdRequest"/> queries.
/// </summary>
public class GetStadiumByIdHandler : IRequestHandler<GetStadiumByIdRequest, GetStadiumByIdResponse>
{
    private readonly IStadiumRepository _stadiumRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetStadiumByIdHandler"/> class.
    /// </summary>
    /// <param name="stadiumRepository">The repository used to load stadiums.</param>
    public GetStadiumByIdHandler(IStadiumRepository stadiumRepository)
    {
        _stadiumRepository = stadiumRepository;
    }

    /// <summary>
    /// Retrieves the stadium identified by the given request.
    /// </summary>
    /// <param name="request">The query request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The response carrying the requested stadium.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when no stadium with the given identifier exists.</exception>
    public async Task<GetStadiumByIdResponse> Handle(GetStadiumByIdRequest request, CancellationToken cancellationToken)
    {
        var stadium = await _stadiumRepository.GetByIdAsync(request.Id)
            ?? throw new KeyNotFoundException($"Stadium {request.Id} not found.");

        return new GetStadiumByIdResponse { Data = GetStadiumByIdMapper.ToDto(stadium) };
    }
}
