using MediatR;

namespace SoccerManager.Application.Commands.MatchTeamTactic.DeleteMatchTeamTactic;

/// <summary>
/// Represents a request to delete an existing match team tactic.
/// </summary>
public sealed record DeleteMatchTeamTacticRequest : IRequest<bool>
{
    /// <summary>
    /// The identifier of the match team tactic to delete.
    /// </summary>
    public int Id { get; init; }
}
