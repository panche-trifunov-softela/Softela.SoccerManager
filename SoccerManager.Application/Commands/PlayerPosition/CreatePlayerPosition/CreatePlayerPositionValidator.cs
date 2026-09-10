using FluentValidation;

namespace SoccerManager.Application.Commands.PlayerPosition.CreatePlayerPosition;

/// <summary>
/// Validates <see cref="CreatePlayerPositionRequest"/> instances.
/// </summary>
public sealed class CreatePlayerPositionValidator : AbstractValidator<CreatePlayerPositionRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreatePlayerPositionValidator"/> class.
    /// </summary>
    public CreatePlayerPositionValidator()
    {
        RuleFor(x => x.PlayerId).GreaterThan(0);
        RuleFor(x => x.PositionId).GreaterThan(0);
        RuleFor(x => x.Quality).InclusiveBetween(1, 100);
    }
}
