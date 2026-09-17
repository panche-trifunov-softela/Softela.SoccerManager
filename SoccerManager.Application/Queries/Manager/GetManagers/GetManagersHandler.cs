using MediatR;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Queries.Manager.GetManagers;

/// <summary>
/// Handles <see cref="GetManagersRequest"/> queries.
/// </summary>
public class GetManagersHandler : IRequestHandler<GetManagersRequest, GetManagersResponse>
{
    private readonly IManagerRepository _managerRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetManagersHandler"/> class.
    /// </summary>
    /// <param name="managerRepository">The repository used to load managers.</param>
    public GetManagersHandler(IManagerRepository managerRepository)
    {
        _managerRepository = managerRepository;
    }

    /// <summary>
    /// Retrieves all managers.
    /// </summary>
    /// <param name="request">The query request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The response carrying every manager.</returns>
    public async Task<GetManagersResponse> Handle(GetManagersRequest request, CancellationToken cancellationToken)
    {
        var managers = await _managerRepository.GetAllAsync();

        return new GetManagersResponse { Data = managers.Select(GetManagersMapper.ToDto).ToList() };
    }
}
