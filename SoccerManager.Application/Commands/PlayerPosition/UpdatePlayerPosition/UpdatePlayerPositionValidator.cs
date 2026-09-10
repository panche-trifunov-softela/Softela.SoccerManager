using FluentValidation;

namespace SoccerManager.Application.Commands.PlayerPosition.UpdatePlayerPosition;

/// <summary>
/// Validates <see cref="UpdatePlayerPositionRequest"/> instances.
/// </summary>
public sealed class UpdatePlayerPositionValidator : AbstractValidator<UpdatePlayerPositionRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdatePlayerPositionValidator"/> class.
    /// </summary>
    public UpdatePlayerPositionValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.PlayerId).GreaterThan(0);
        RuleFor(x => x.PositionId).GreaterThan(0);
        RuleFor(x => x.Quality).InclusiveBetween(1, 100);
    }
}
