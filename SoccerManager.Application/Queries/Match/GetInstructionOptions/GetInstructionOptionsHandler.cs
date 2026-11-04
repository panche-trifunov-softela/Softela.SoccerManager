using MediatR;
using SoccerManager.Application.Dtos;
using SoccerManager.Domain.Enums;

namespace SoccerManager.Application.Queries.Match.GetInstructionOptions;

/// <summary>
/// Handles <see cref="GetInstructionOptionsRequest"/> queries.
/// </summary>
public class GetInstructionOptionsHandler : IRequestHandler<GetInstructionOptionsRequest, GetInstructionOptionsResponse>
{
    /// <summary>
    /// Builds the instructions from their enums. Nothing is read, because the enums are the only list of them.
    /// </summary>
    /// <param name="request">The query request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The response carrying the instructions.</returns>
    public Task<GetInstructionOptionsResponse> Handle(GetInstructionOptionsRequest request, CancellationToken cancellationToken)
    {
        var instructions = new List<InstructionDefinitionDto>
        {
            GetInstructionOptionsMapper.ToDto<Mentality>(),
            GetInstructionOptionsMapper.ToDto<Tempo>(),
            GetInstructionOptionsMapper.ToDto<Passing>(),
            GetInstructionOptionsMapper.ToDto<Width>(),
            GetInstructionOptionsMapper.ToDto<Pressing>(),
            GetInstructionOptionsMapper.ToDto<Tackling>(),
            GetInstructionOptionsMapper.ToDto<AttackingSide>(),
        };

        return Task.FromResult(new GetInstructionOptionsResponse { Data = instructions });
    }
}
