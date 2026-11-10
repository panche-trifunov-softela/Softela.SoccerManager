using MediatR;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Queries.PenaltyAfterExtraTime.GetPenaltyAfterExtraTimeById;

/// <summary>
/// Handles <see cref="GetPenaltyAfterExtraTimeByIdRequest"/> queries.
/// </summary>
public class GetPenaltyAfterExtraTimeByIdHandler : IRequestHandler<GetPenaltyAfterExtraTimeByIdRequest, GetPenaltyAfterExtraTimeByIdResponse>
{
    private readonly IPenaltyAfterExtraTimeRepository _penaltyAfterExtraTimeRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetPenaltyAfterExtraTimeByIdHandler"/> class.
    /// </summary>
    /// <param name="penaltyAfterExtraTimeRepository">The repository used to load penalties after extra time.</param>
    public GetPenaltyAfterExtraTimeByIdHandler(IPenaltyAfterExtraTimeRepository penaltyAfterExtraTimeRepository)
    {
        _penaltyAfterExtraTimeRepository = penaltyAfterExtraTimeRepository;
    }

    /// <summary>
    /// Retrieves the penalty after extra time identified by the given request.
    /// </summary>
    /// <param name="request">The query request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The response carrying the requested penalty after extra time.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when no penalty after extra time with the given identifier exists.</exception>
    public async Task<GetPenaltyAfterExtraTimeByIdResponse> Handle(GetPenaltyAfterExtraTimeByIdRequest request, CancellationToken cancellationToken)
    {
        var penaltyAfterExtraTime = await _penaltyAfterExtraTimeRepository.GetByIdAsync(request.Id)
            ?? throw new KeyNotFoundException($"Penalty after extra time {request.Id} not found.");

        return new GetPenaltyAfterExtraTimeByIdResponse { Data = GetPenaltyAfterExtraTimeByIdMapper.ToDto(penaltyAfterExtraTime) };
    }
}
