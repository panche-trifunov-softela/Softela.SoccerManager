namespace SoccerManager.Application.Dtos;

/// <summary>
/// Represents one choice of an instruction: the number the API uses for it and the words that name it.
/// </summary>
public sealed record InstructionOptionDto
{
    /// <summary>
    /// The number the API sends and takes for the choice.
    /// </summary>
    public int Value { get; init; }

    /// <summary>
    /// What the choice is called on screen, for example <c>High press</c>.
    /// </summary>
    public required string Label { get; init; }
}
