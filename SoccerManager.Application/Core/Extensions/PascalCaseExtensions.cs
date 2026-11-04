namespace SoccerManager.Application.Core.Extensions;

/// <summary>
/// Reads a PascalCase code name, such as an enum member's, as the words a person sees.
/// </summary>
public static class PascalCaseExtensions
{
    /// <summary>
    /// Converts a PascalCase name into words, keeping only the first capital.
    /// </summary>
    /// <param name="name">The PascalCase name.</param>
    /// <returns>The name in words.</returns>
    public static string ToWords(this string name)
    {
        // Each capital after the first starts a new lower-case word: HighPress reads as "High press".
        var words = name.Select((letter, index) =>
            index > 0 && char.IsUpper(letter) ? $" {char.ToLowerInvariant(letter)}" : letter.ToString());

        return string.Concat(words);
    }
}
