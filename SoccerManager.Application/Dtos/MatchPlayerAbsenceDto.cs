using SoccerManager.Domain.Enums;

namespace SoccerManager.Application.Dtos;

/// <summary>
/// Represents a match player absence for read-oriented consumers.
/// </summary>
public sealed record MatchPlayerAbsenceDto
{
    /// <summary>
    /// The identifier of the match player absence.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// The identifier of the match the player missed.
    /// </summary>
    public int MatchId { get; init; }

    /// <summary>
    /// The identifier of the league team (the club in its league) the player was missing from.
    /// </summary>
    public int LeagueTeamId { get; init; }

    /// <summary>
    /// The identifier of the player who was absent.
    /// </summary>
    public int PlayerId { get; init; }

    /// <summary>
    /// Why the player missed the match.
    /// </summary>
    public MatchAbsenceReason Reason { get; init; }

    /// <summary>
    /// The reason in words, "Injured" or "Suspended", worked out from <see cref="Reason"/>.
    /// </summary>
    public required string ReasonLabel { get; init; }

    /// <summary>
    /// The UTC date and time the match player absence was created.
    /// </summary>
    public DateTime CreatedAt { get; init; }

    /// <summary>
    /// The UTC date and time the match player absence was last modified.
    /// </summary>
    public DateTime ModifiedAt { get; init; }
}
