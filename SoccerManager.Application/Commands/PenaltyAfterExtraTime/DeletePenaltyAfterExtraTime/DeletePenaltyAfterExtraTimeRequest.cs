using MediatR;

namespace SoccerManager.Application.Commands.PenaltyAfterExtraTime.DeletePenaltyAfterExtraTime;

/// <summary>
/// Represents a request to delete an existing penalty after extra time.
/// </summary>
public sealed record DeletePenaltyAfterExtraTimeRequest : IRequest<bool>
{
    /// <summary>
    /// The identifier of the penalty after extra time to delete.
    /// </summary>
    public int Id { get; init; }
}
