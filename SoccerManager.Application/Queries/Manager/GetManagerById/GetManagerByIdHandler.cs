using MediatR;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Queries.Manager.GetManagerById;

/// <summary>
/// Handles <see cref="GetManagerByIdRequest"/> queries.
/// </summary>
public class GetManagerByIdHandler : IRequestHandler<GetManagerByIdRequest, GetManagerByIdResponse>
{
    private readonly IManagerRepository _managerRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetManagerByIdHandler"/> class.
    /// </summary>
    /// <param name="managerRepository">The repository used to load managers.</param>
    public GetManagerByIdHandler(IManagerRepository managerRepository)
    {
        _managerRepository = managerRepository;
    }

    /// <summary>
    /// Retrieves the manager profile identified by the given request.
    /// </summary>
    /// <param name="request">The query request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The response carrying the requested manager profile.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when no manager with the given identifier exists.</exception>
    public async Task<GetManagerByIdResponse> Handle(GetManagerByIdRequest request, CancellationToken cancellationToken)
    {
        var manager = await _managerRepository.GetByIdAsync(request.Id)
            ?? throw new KeyNotFoundException($"Manager {request.Id} not found.");

        return new GetManagerByIdResponse { Data = GetManagerByIdMapper.ToDto(manager) };
    }
}
