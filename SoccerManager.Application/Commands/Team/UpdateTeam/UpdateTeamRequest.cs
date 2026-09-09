using MediatR;
using SoccerManager.Domain.Enums;

namespace SoccerManager.Application.Commands.Team.UpdateTeam;

/// <summary>
/// Represents a request to update an existing team.
/// </summary>
public sealed record UpdateTeamRequest : IRequest<bool>
{
    /// <summary>
    /// The identifier of the team to update.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// The new name of the team.
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
}
