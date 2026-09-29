using MediatR;
using SoccerManager.Domain.Enums;

namespace SoccerManager.Application.Commands.Team.CreateTeam;

/// <summary>
/// Represents a request to create a new team.
/// </summary>
public sealed record CreateTeamRequest : IRequest<int>
{
    /// <summary>
    /// The name of the team to create.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// The identifier of the stadium the team plays at, if one has been assigned.
    /// </summary>
    public int? StadiumId { get; init; }

    /// <summary>
    /// The financial state of the team.
    /// </summary>
    public FinancialState FinancialState { get; init; }

    /// <summary>
    /// The URL of the team's jersey image, if one has been set.
    /// </summary>
    public string? JerseyUrl { get; init; }

    /// <summary>
    /// The URL of the team's logo image, if one has been set.
    /// </summary>
    public string? LogoUrl { get; init; }

    /// <summary>
    /// The team's Transfermarkt identifier, if one has been assigned.
    /// </summary>
    public int? TransfermarktId { get; init; }
}
