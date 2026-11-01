namespace SoccerManager.Application.Commands.Manager.EnsureMyManager;

/// <summary>
/// Maps an <see cref="EnsureMyManagerRequest"/> to the manager profile it creates when the caller has none.
/// </summary>
public static class EnsureMyManagerMapper
{
    /// <summary>
    /// The longest first or last name stored: dbo.Managers declares both as NVARCHAR(255) in V1_0_0_49, the same
    /// limit as Keycloak's default user profile.
    /// </summary>
    private const int MaxNameLength = 255;

    /// <summary>
    /// Creates the caller's manager profile with no image and the first and last name from their token, as it is stored
    /// on a first login; on later logins only a changed name is written, together with the modification stamp.
    /// </summary>
    /// <param name="now">The UTC timestamp to stamp on the created and modified fields.</param>
    /// <param name="userId">The identifier of the authenticated caller, who owns the profile.</param>
    /// <param name="firstName">The first name from the caller's token, or <see langword="null"/> when absent.</param>
    /// <param name="lastName">The last name from the caller's token, or <see langword="null"/> when absent.</param>
    /// <returns>A new, unsaved manager entity.</returns>
    public static SoccerManager.Domain.Entities.Manager ToDomainEntity(
        DateTime now,
        Guid userId,
        string? firstName,
        string? lastName)
    {
        return new SoccerManager.Domain.Entities.Manager
        {
            UserId = userId,
            ImageUrl = null,
            FirstName = ToStoredName(firstName),
            LastName = ToStoredName(lastName),
            CreatedAt = now,
            CreatedBy = userId,
            ModifiedAt = now,
            ModifiedBy = userId,
        };
    }

    /// <summary>
    /// Trims a name from the token and cuts an over-long one to <see cref="MaxNameLength"/> without splitting a
    /// surrogate pair.
    /// </summary>
    /// <param name="name">The name from the token, or <see langword="null"/> when it carries none.</param>
    /// <returns><see langword="null"/> when the name is missing or blank; otherwise the trimmed name.</returns>
    private static string? ToStoredName(string? name)
    {
        var trimmed = name?.Trim();

        if (string.IsNullOrEmpty(trimmed))
        {
            return null;
        }

        if (trimmed.Length > MaxNameLength)
        {
            // Cutting between the two halves of a surrogate pair would store half a character.
            var length = char.IsHighSurrogate(trimmed[MaxNameLength - 1]) ? MaxNameLength - 1 : MaxNameLength;

            trimmed = trimmed[..length].TrimEnd();
        }

        return trimmed;
    }
}
