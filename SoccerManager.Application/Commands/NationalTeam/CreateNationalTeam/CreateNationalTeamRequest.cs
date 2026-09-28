using MediatR;

namespace SoccerManager.Application.Commands.NationalTeam.CreateNationalTeam;

/// <summary>
/// Represents a request to create a new national team.
/// </summary>
public sealed record CreateNationalTeamRequest : IRequest<int>
{
    /// <summary>
    /// The name of the national team to create.
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
