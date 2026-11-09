using MediatR;

namespace SoccerManager.Application.Queries.Match.GetMatchSubstitutions;

/// <summary>
/// Represents a request to retrieve a club's substitutions for a match.
/// </summary>
public sealed record GetMatchSubstitutionsRequest : IRequest<GetMatchSubstitutionsResponse>
{
    /// <summary>
    /// The identifier of the match the substitutions are for.
    /// </summary>
    public int MatchId { get; init; }

    /// <summary>
    /// The identifier of the league team (the club in its league) whose substitutions to retrieve.
    /// </summary>
    public int LeagueTeamId { get; init; }
}
