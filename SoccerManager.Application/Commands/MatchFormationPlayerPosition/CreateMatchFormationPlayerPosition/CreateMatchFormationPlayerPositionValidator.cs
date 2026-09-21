using FluentValidation;

namespace SoccerManager.Application.Commands.MatchFormationPlayerPosition.CreateMatchFormationPlayerPosition;

/// <summary>
/// Validates <see cref="CreateMatchFormationPlayerPositionRequest"/> instances.
/// </summary>
public sealed class CreateMatchFormationPlayerPositionValidator : AbstractValidator<CreateMatchFormationPlayerPositionRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateMatchFormationPlayerPositionValidator"/> class.
    /// </summary>
    public CreateMatchFormationPlayerPositionValidator()
    {
        RuleFor(x => x.MatchId).GreaterThan(0);
        RuleFor(x => x.TeamId).GreaterThan(0);
        RuleFor(x => x.FormationPositionId).GreaterThan(0);
        RuleFor(x => x.PlayerPositionId).GreaterThan(0);
    }
}
