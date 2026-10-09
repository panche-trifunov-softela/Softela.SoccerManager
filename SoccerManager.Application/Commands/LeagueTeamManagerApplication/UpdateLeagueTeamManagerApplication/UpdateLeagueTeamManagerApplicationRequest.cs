using MediatR;
using SoccerManager.Domain.Enums;

namespace SoccerManager.Application.Commands.LeagueTeamManagerApplication.UpdateLeagueTeamManagerApplication;

/// <summary>
/// Represents a request to answer a pending league team manager application, by accepting or rejecting it.
/// </summary>
public sealed record UpdateLeagueTeamManagerApplicationRequest : IRequest<bool>
{
    /// <summary>
    /// The identifier of the league team manager application to answer.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// The answer to give: <see cref="ApplicationStatus.Accepted"/> or <see cref="ApplicationStatus.Rejected"/>.
    /// There is deliberately no default, so an omitted value is zero and fails validation.
    /// </summary>
    public ApplicationStatus Status { get; init; }
}
