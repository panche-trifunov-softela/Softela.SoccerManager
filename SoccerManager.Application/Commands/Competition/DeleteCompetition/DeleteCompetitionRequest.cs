using MediatR;

namespace SoccerManager.Application.Commands.Competition.DeleteCompetition;

/// <summary>
/// Represents a request to delete an existing competition.
/// </summary>
public sealed record DeleteCompetitionRequest : IRequest<bool>
{
    /// <summary>
    /// The identifier of the competition to delete.
    /// </summary>
    public int Id { get; init; }
}
