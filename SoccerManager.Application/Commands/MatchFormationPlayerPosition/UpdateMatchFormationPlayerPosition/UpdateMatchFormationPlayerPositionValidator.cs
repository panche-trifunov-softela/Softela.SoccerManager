using FluentValidation;

namespace SoccerManager.Application.Commands.MatchFormationPlayerPosition.UpdateMatchFormationPlayerPosition;

/// <summary>
/// Validates <see cref="UpdateMatchFormationPlayerPositionRequest"/> instances.
/// </summary>
public sealed class UpdateMatchFormationPlayerPositionValidator : AbstractValidator<UpdateMatchFormationPlayerPositionRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateMatchFormationPlayerPositionValidator"/> class.
    /// </summary>
    public UpdateMatchFormationPlayerPositionValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.MatchId).GreaterThan(0);
        RuleFor(x => x.TeamId).GreaterThan(0);
        RuleFor(x => x.FormationPositionId).GreaterThan(0);
        RuleFor(x => x.PlayerPositionId).GreaterThan(0);
    }
}
