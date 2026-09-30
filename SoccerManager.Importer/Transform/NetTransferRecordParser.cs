using System.Globalization;

namespace SoccerManager.Importer.Transform;

/// <summary>
/// Parses a club's raw Transfermarkt net transfer record text into a signed EUR amount.
/// </summary>
public static class NetTransferRecordParser
{
    /// <summary>
    /// Parses <paramref name="raw"/> into a signed EUR amount. Spaces are removed; <c>"+-0"</c> means zero; a
    /// leading '-' before the digits makes the amount negative; the suffixes k/m/bn scale by 1e3/1e6/1e9; the euro
    /// sign and a leading '+' are ignored. Anything that still does not parse as a number returns
    /// <see langword="null"/>.
    /// </summary>
    /// <param name="raw">The raw net transfer record text, as recorded on Transfermarkt.</param>
    /// <returns>The parsed EUR amount, or <see langword="null"/> when <paramref name="raw"/> is blank or unparseable.</returns>
    public static decimal? Parse(string? raw)
    {
        if (string.IsNullOrWhiteSpace(raw))
        {
            return null;
        }

        var text = raw.Replace(" ", string.Empty, StringComparison.Ordinal);

        if (string.Equals(text, "+-0", StringComparison.Ordinal))
        {
            return 0m;
        }

        text = text.Replace("€", string.Empty, StringComparison.Ordinal);
        var negative = text.Contains('-');
        text = text.Replace("+", string.Empty, StringComparison.Ordinal).Replace("-", string.Empty, StringComparison.Ordinal);

        var multiplier = 1m;
        if (text.EndsWith("bn", StringComparison.OrdinalIgnoreCase))
        {
            multiplier = 1_000_000_000m;
            text = text[..^2];
        }
        else if (text.EndsWith("m", StringComparison.OrdinalIgnoreCase))
        {
            multiplier = 1_000_000m;
            text = text[..^1];
        }
        else if (text.EndsWith("k", StringComparison.OrdinalIgnoreCase))
        {
            multiplier = 1_000m;
            text = text[..^1];
        }

        if (!decimal.TryParse(text, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out var magnitude))
        {
            return null;
        }

        var amount = magnitude * multiplier;
        return negative ? -amount : amount;
    }
}
