using MediatR;

namespace SoccerManager.Application.Queries.MatchTeamTactic.GetMatchTeamTacticById;

/// <summary>
/// Represents a request to retrieve a single match team tactic by identifier.
/// </summary>
public sealed record GetMatchTeamTacticByIdRequest : IRequest<GetMatchTeamTacticByIdResponse>
{
    /// <summary>
    /// The identifier of the match team tactic to retrieve.
    /// </summary>
    public int Id { get; init; }
}
