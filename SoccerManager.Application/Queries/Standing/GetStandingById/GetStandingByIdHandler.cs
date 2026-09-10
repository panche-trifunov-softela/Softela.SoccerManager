using MediatR;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Queries.Standing.GetStandingById;

/// <summary>
/// Handles <see cref="GetStandingByIdRequest"/> queries.
/// </summary>
public class GetStandingByIdHandler : IRequestHandler<GetStandingByIdRequest, GetStandingByIdResponse>
{
    private readonly IStandingRepository _standingRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetStandingByIdHandler"/> class.
    /// </summary>
    /// <param name="standingRepository">The repository used to load standings.</param>
    public GetStandingByIdHandler(IStandingRepository standingRepository)
    {
        _standingRepository = standingRepository;
    }

    /// <summary>
    /// Retrieves the standing identified by the given request.
    /// </summary>
    /// <param name="request">The query request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The response carrying the requested standing.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when no standing with the given identifier exists.</exception>
    public async Task<GetStandingByIdResponse> Handle(GetStandingByIdRequest request, CancellationToken cancellationToken)
    {
        var standing = await _standingRepository.GetByIdAsync(request.Id)
            ?? throw new KeyNotFoundException($"Standing {request.Id} not found.");

        return new GetStandingByIdResponse { Data = GetStandingByIdMapper.ToDto(standing) };
    }
}
