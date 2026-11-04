namespace SoccerManager.Application.Dtos;

/// <summary>
/// Represents one of a club's instructions for a match as a client offers it: which one it is, what it is called and its
/// choices.
/// </summary>
public sealed record InstructionDefinitionDto
{
    /// <summary>
    /// The instruction's name in <see cref="MatchInstructionsDto"/> as the API writes it, for example <c>attackingSide</c>.
    /// </summary>
    public required string Field { get; init; }

    /// <summary>
    /// What the instruction is called on screen, for example <c>Attacking side</c>.
    /// </summary>
    public required string Label { get; init; }

    /// <summary>
    /// The instruction's choices, lowest number first.
    /// </summary>
    public required List<InstructionOptionDto> Options { get; init; }
}
