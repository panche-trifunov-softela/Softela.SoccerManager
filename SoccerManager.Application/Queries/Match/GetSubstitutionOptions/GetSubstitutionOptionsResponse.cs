using SoccerManager.Application.Dtos;

namespace SoccerManager.Application.Queries.Match.GetSubstitutionOptions;

/// <summary>
/// Represents the result of a <see cref="GetSubstitutionOptionsRequest"/> query.
/// </summary>
public sealed record GetSubstitutionOptionsResponse
{
    /// <summary>
    /// The minute, then the situation, each with its choices.
    /// </summary>
    public required List<InstructionDefinitionDto> Data { get; init; }
}
