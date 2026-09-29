using MediatR;
using SoccerManager.Domain.Enums;

namespace SoccerManager.Application.Commands.Referee.CreateReferee;

/// <summary>
/// Represents a request to create a new referee.
/// </summary>
public sealed record CreateRefereeRequest : IRequest<int>
{
    /// <summary>
    /// The name of the referee to create.
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
