namespace SoccerManager.Importer.Database.Changes;

/// <summary>
/// One field an update would change, for the database changes report.
/// </summary>
/// <param name="Field">The field's name, as it appears on the request type.</param>
/// <param name="Before">The field's current database value, still typed (an int, a decimal, a <see cref="DateOnly"/>, a string, an enum, or a display name for a link). <see langword="null"/> when the database has no value.</param>
/// <param name="After">The field's new value, typed the same way as <paramref name="Before"/>.</param>
public sealed record FieldChange(string Field, object? Before, object? After);
