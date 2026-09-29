using MediatR;
using SoccerManager.Domain.Enums;

namespace SoccerManager.Application.Commands.Referee.UpdateReferee;

/// <summary>
/// Represents a request to update an existing referee.
/// </summary>
public sealed record UpdateRefereeRequest : IRequest<bool>
{
    /// <summary>
    /// The identifier of the referee to update.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// The new name of the referee.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// The URL of the referee's image, if one has been set.
    /// </summary>
    public string? ImageUrl { get; init; }

    /// <summary>
    /// How much foul play the referee lets go before penalizing it; <c>Balanced</c> when omitted.
    /// </summary>
    public Tolerance Tolerance { get; init; } = Tolerance.Balanced;
}
