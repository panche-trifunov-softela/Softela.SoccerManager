using MediatR;
using SoccerManager.Domain.Enums;

namespace SoccerManager.Application.Commands.Competition.CreateCompetition;

/// <summary>
/// Represents a request to create a new competition.
/// </summary>
public sealed record CreateCompetitionRequest : IRequest<int>
{
    /// <summary>
    /// The identifier of the league the competition belongs to.
    /// </summary>
    public int LeagueId { get; init; }

    /// <summary>
    /// The name of the competition to create.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// The URL of the competition's logo image, if one has been set.
    /// </summary>
    public string? LogoUrl { get; init; }

    /// <summary>
    /// Whether the competition is domestic.
    /// </summary>
    public bool IsDomestic { get; init; }

    /// <summary>
    /// The format of the competition.
    /// </summary>
    public CompetitionFormat Format { get; init; }

    /// <summary>
    /// The maximum age allowed to participate in the competition, if one has been set. Null means no age limit.
    /// </summary>
    public int? MaxAgeAllowed { get; init; }
}
