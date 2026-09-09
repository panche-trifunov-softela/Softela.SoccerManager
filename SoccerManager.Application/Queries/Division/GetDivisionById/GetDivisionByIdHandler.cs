using MediatR;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Queries.Division.GetDivisionById;

/// <summary>
/// Handles <see cref="GetDivisionByIdRequest"/> queries.
/// </summary>
public class GetDivisionByIdHandler : IRequestHandler<GetDivisionByIdRequest, GetDivisionByIdResponse>
{
    private readonly IDivisionRepository _divisionRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetDivisionByIdHandler"/> class.
    /// </summary>
    /// <param name="divisionRepository">The repository used to load divisions.</param>
    public GetDivisionByIdHandler(IDivisionRepository divisionRepository)
    {
        _divisionRepository = divisionRepository;
    }

    /// <summary>
    /// Retrieves the division identified by the given request.
    /// </summary>
    /// <param name="request">The query request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The response carrying the requested division.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when no division with the given identifier exists.</exception>
    public async Task<GetDivisionByIdResponse> Handle(GetDivisionByIdRequest request, CancellationToken cancellationToken)
    {
        var division = await _divisionRepository.GetByIdAsync(request.Id)
            ?? throw new KeyNotFoundException($"Division {request.Id} not found.");

        return new GetDivisionByIdResponse { Data = GetDivisionByIdMapper.ToDto(division) };
    }
}
