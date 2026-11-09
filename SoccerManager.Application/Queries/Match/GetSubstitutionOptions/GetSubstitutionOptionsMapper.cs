using System.Globalization;
using System.Text.Json;
using SoccerManager.Application.Core.Extensions;
using SoccerManager.Application.Dtos;

namespace SoccerManager.Application.Queries.Match.GetSubstitutionOptions;

/// <summary>
/// Maps a substitution's minutes to an <see cref="InstructionDefinitionDto"/>, the shape the instruction options use, so a
/// client offers both lists the same way.
/// </summary>
public static class GetSubstitutionOptionsMapper
{
    /// <summary>
    /// Converts the minutes a substitution can take effect at into the DTO representation: the field is
    /// <see cref="MatchSubstitutionDto.MinuteAt"/> as the API writes it, and each minute is a choice labelled with its number.
    /// </summary>
    /// <param name="minutes">The minutes a substitution can take effect at.</param>
    /// <returns>The corresponding <see cref="InstructionDefinitionDto"/>.</returns>
    public static InstructionDefinitionDto ToMinuteDto(IReadOnlyList<int> minutes)
    {
        const string name = nameof(MatchSubstitutionDto.MinuteAt);

        return new InstructionDefinitionDto
        {
            // The policy the API writes its JSON with, so the field is the name a client finds in MatchSubstitutionDto.
            Field = JsonNamingPolicy.CamelCase.ConvertName(name),
            Label = name.ToWords(),
            Options = minutes
                .Select(minute => new InstructionOptionDto { Value = minute, Label = minute.ToString(CultureInfo.InvariantCulture) })
                .ToList(),
        };
    }
}
