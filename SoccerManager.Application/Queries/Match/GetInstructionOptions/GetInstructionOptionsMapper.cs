using System.Text.Json;
using SoccerManager.Application.Dtos;

namespace SoccerManager.Application.Queries.Match.GetInstructionOptions;

/// <summary>
/// Maps an instruction's enum to its <see cref="InstructionDefinitionDto"/>, working out the words from the code names, so a
/// new enum member is offered with no other change.
/// </summary>
public static class GetInstructionOptionsMapper
{
    /// <summary>
    /// Converts an instruction's enum into its DTO representation: the enum is the instruction and its members are the choices.
    /// </summary>
    /// <typeparam name="TInstruction">The instruction's enum, named like its property on <see cref="MatchInstructionsDto"/>.</typeparam>
    /// <returns>The corresponding <see cref="InstructionDefinitionDto"/>.</returns>
    public static InstructionDefinitionDto ToDto<TInstruction>()
        where TInstruction : struct, Enum
    {
        var name = typeof(TInstruction).Name;

        return new InstructionDefinitionDto
        {
            // The policy the API writes its JSON with, so the field is the name a client finds in MatchInstructionsDto.
            Field = JsonNamingPolicy.CamelCase.ConvertName(name),
            Label = ToWords(name),
            Options = Enum.GetValues<TInstruction>()
                .Select(member => new InstructionOptionDto { Value = Convert.ToInt32(member), Label = ToWords(member.ToString()) })
                .ToList(),
        };
    }

    private static string ToWords(string name)
    {
        // Each capital after the first starts a new lower-case word: HighPress reads as "High press".
        var words = name.Select((letter, index) =>
            index > 0 && char.IsUpper(letter) ? $" {char.ToLowerInvariant(letter)}" : letter.ToString());

        return string.Concat(words);
    }
}
