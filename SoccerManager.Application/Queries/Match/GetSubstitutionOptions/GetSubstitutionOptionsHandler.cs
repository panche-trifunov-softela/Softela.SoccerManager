using MediatR;
using SoccerManager.Application.Dtos;
using SoccerManager.Application.Queries.Match.GetInstructionOptions;
using SoccerManager.Application.Services;
using SoccerManager.Domain.Enums;

namespace SoccerManager.Application.Queries.Match.GetSubstitutionOptions;

/// <summary>
/// Handles <see cref="GetSubstitutionOptionsRequest"/> queries.
/// </summary>
public class GetSubstitutionOptionsHandler : IRequestHandler<GetSubstitutionOptionsRequest, GetSubstitutionOptionsResponse>
{
    /// <summary>
    /// Builds the minute and situation choices from the substitution limits and the situation enum. Nothing is read, because
    /// those are the only list of them.
    /// </summary>
    /// <param name="request">The query request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The response carrying the minute, then the situation, each with its choices.</returns>
    public Task<GetSubstitutionOptionsResponse> Handle(GetSubstitutionOptionsRequest request, CancellationToken cancellationToken)
    {
        var options = new List<InstructionDefinitionDto>
        {
            GetSubstitutionOptionsMapper.ToMinuteDto(MatchSubstitutionLimits.Minutes),
            GetInstructionOptionsMapper.ToDto<Situation>(),
        };

        return Task.FromResult(new GetSubstitutionOptionsResponse { Data = options });
    }
}
