using MediatR;
using SoccerManager.Application.Dtos;

namespace SoccerManager.Application.Commands.Match.AutoPickMatchLineup;

/// <summary>
/// Represents a request to replace a club's lineup for a match with an automatically picked one.
/// </summary>
public sealed record AutoPickMatchLineupRequest : IRequest<MatchLineupDto>
{
    /// <summary>
    /// The identifier of the match the lineup is for. The route supplies it and overwrites a value in the body.
    /// </summary>
    public int MatchId { get; init; }

    /// <summary>
    /// The identifier of the league team (the club in its league) whose lineup to pick.
    /// </summary>
    public int LeagueTeamId { get; init; }

    /// <summary>
    /// The identifier of the formation to pick for, or <see langword="null"/> to keep the stored one, or use the default when
    /// there is none.
    /// </summary>
    public int? FormationId { get; init; }
}
