using System.Globalization;
using System.Text;

namespace SoccerManager.Importer.Transform;

/// <summary>
/// Builds a de-duplication key from a name: two names that differ only in case, accents or incidental whitespace
/// normalize to the same key.
/// </summary>
public static class NameNormalizer
{
    /// <summary>
    /// Normalizes <paramref name="name"/> into a de-duplication key: trims it, lower-cases it (invariant), strips
    /// diacritics, keeps only letters, digits and spaces, and collapses runs of whitespace to a single space.
    /// </summary>
    /// <param name="name">The name to normalize.</param>
    /// <returns>The normalized key.</returns>
    public static string Normalize(string name)
    {
        ArgumentNullException.ThrowIfNull(name);

        var lowered = name.Trim().ToLowerInvariant();
        var decomposed = lowered.Normalize(NormalizationForm.FormD);

        var builder = new StringBuilder(decomposed.Length);
        foreach (var character in decomposed)
        {
            if (CharUnicodeInfo.GetUnicodeCategory(character) == UnicodeCategory.NonSpacingMark)
            {
                continue;
            }

            if (char.IsLetterOrDigit(character) || char.IsWhiteSpace(character))
            {
                builder.Append(character);
            }
        }

        var collapsed = new StringBuilder(builder.Length);
        var lastWasSpace = false;
        foreach (var character in builder.ToString())
        {
            var isSpace = char.IsWhiteSpace(character);
            if (isSpace && lastWasSpace)
            {
                continue;
            }

            collapsed.Append(isSpace ? ' ' : character);
            lastWasSpace = isSpace;
        }

        return collapsed.ToString().Trim();
    }
}
