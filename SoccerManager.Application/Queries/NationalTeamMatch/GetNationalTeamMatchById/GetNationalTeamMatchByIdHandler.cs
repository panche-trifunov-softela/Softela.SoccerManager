using MediatR;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Queries.NationalTeamMatch.GetNationalTeamMatchById;

/// <summary>
/// Handles <see cref="GetNationalTeamMatchByIdRequest"/> queries.
/// </summary>
public class GetNationalTeamMatchByIdHandler : IRequestHandler<GetNationalTeamMatchByIdRequest, GetNationalTeamMatchByIdResponse>
{
    private readonly INationalTeamMatchRepository _nationalTeamMatchRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetNationalTeamMatchByIdHandler"/> class.
    /// </summary>
    /// <param name="nationalTeamMatchRepository">The repository used to load national team matches.</param>
    public GetNationalTeamMatchByIdHandler(INationalTeamMatchRepository nationalTeamMatchRepository)
    {
        _nationalTeamMatchRepository = nationalTeamMatchRepository;
    }

    /// <summary>
    /// Retrieves the national team match identified by the given request.
    /// </summary>
    /// <param name="request">The query request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The response carrying the requested national team match.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when no national team match with the given identifier exists.</exception>
    public async Task<GetNationalTeamMatchByIdResponse> Handle(GetNationalTeamMatchByIdRequest request, CancellationToken cancellationToken)
    {
        var nationalTeamMatch = await _nationalTeamMatchRepository.GetByIdAsync(request.Id)
            ?? throw new KeyNotFoundException($"National team match {request.Id} not found.");

        return new GetNationalTeamMatchByIdResponse { Data = GetNationalTeamMatchByIdMapper.ToDto(nationalTeamMatch) };
    }
}
