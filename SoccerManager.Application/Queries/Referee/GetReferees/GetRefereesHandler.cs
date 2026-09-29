using MediatR;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Queries.Referee.GetReferees;

/// <summary>
/// Handles <see cref="GetRefereesRequest"/> queries.
/// </summary>
public class GetRefereesHandler : IRequestHandler<GetRefereesRequest, GetRefereesResponse>
{
    private readonly IRefereeRepository _refereeRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetRefereesHandler"/> class.
    /// </summary>
    /// <param name="refereeRepository">The repository used to load referees.</param>
    public GetRefereesHandler(IRefereeRepository refereeRepository)
    {
        _refereeRepository = refereeRepository;
    }

    /// <summary>
    /// Retrieves all referees.
    /// </summary>
    /// <param name="request">The query request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The response carrying every referee.</returns>
    public async Task<GetRefereesResponse> Handle(GetRefereesRequest request, CancellationToken cancellationToken)
    {
        var referees = await _refereeRepository.GetAllAsync();

        return new GetRefereesResponse { Data = referees.Select(GetRefereesMapper.ToDto).ToList() };
    }
}
