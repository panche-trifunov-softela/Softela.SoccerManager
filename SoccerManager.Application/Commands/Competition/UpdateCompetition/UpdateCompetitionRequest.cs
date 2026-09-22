using MediatR;
using SoccerManager.Domain.Enums;

namespace SoccerManager.Application.Commands.Competition.UpdateCompetition;

/// <summary>
/// Represents a request to update an existing competition. LeagueId is intentionally absent:
/// it is fixed at creation, so a competition never moves between leagues.
/// </summary>
public sealed record UpdateCompetitionRequest : IRequest<bool>
{
    /// <summary>
    /// The identifier of the competition to update.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// The new name of the competition.
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
    /// The maximum age allowed to participate in the competition, or <see langword="null"/>
    /// when there is no age limit.
    /// </summary>
    public int? MaxAgeAllowed { get; init; }
}
