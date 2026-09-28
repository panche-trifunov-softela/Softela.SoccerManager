using MediatR;

namespace SoccerManager.Application.Commands.NationalTeam.UpdateNationalTeam;

/// <summary>
/// Represents a request to update an existing national team.
/// </summary>
public sealed record UpdateNationalTeamRequest : IRequest<bool>
{
    /// <summary>
    /// The identifier of the national team to update.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// The new name of the national team.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// The identifier of the stadium the national team plays at, if one has been assigned.
    /// </summary>
    public int? StadiumId { get; init; }

    /// <summary>
    /// The URL of the national team's jersey image, if one has been set.
    /// </summary>
    public string? JerseyUrl { get; init; }

    /// <summary>
    /// The URL of the national team's logo image, if one has been set.
    /// </summary>
    public string? LogoUrl { get; init; }
}
