using SoccerManager.Application.Dtos;

namespace SoccerManager.Application.Queries.Match.GetInstructionOptions;

/// <summary>
/// Represents the result of a <see cref="GetInstructionOptionsRequest"/> query.
/// </summary>
public sealed record GetInstructionOptionsResponse
{
    /// <summary>
    /// The instructions, in the order a client lays them out.
    /// </summary>
    public required List<InstructionDefinitionDto> Data { get; init; }
}
