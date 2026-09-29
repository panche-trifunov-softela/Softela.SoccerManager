using MediatR;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Queries.Referee.GetRefereeById;

/// <summary>
/// Handles <see cref="GetRefereeByIdRequest"/> queries.
/// </summary>
public class GetRefereeByIdHandler : IRequestHandler<GetRefereeByIdRequest, GetRefereeByIdResponse>
{
    private readonly IRefereeRepository _refereeRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetRefereeByIdHandler"/> class.
    /// </summary>
    /// <param name="refereeRepository">The repository used to load referees.</param>
    public GetRefereeByIdHandler(IRefereeRepository refereeRepository)
    {
        _refereeRepository = refereeRepository;
    }

    /// <summary>
    /// Retrieves the referee identified by the given request.
    /// </summary>
    /// <param name="request">The query request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The response carrying the requested referee.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when no referee with the given identifier exists.</exception>
    public async Task<GetRefereeByIdResponse> Handle(GetRefereeByIdRequest request, CancellationToken cancellationToken)
    {
        var referee = await _refereeRepository.GetByIdAsync(request.Id)
            ?? throw new KeyNotFoundException($"Referee {request.Id} not found.");

        return new GetRefereeByIdResponse { Data = GetRefereeByIdMapper.ToDto(referee) };
    }
}
