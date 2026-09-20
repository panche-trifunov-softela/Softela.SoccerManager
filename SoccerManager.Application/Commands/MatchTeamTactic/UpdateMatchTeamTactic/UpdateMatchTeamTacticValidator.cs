using FluentValidation;

namespace SoccerManager.Application.Commands.MatchTeamTactic.UpdateMatchTeamTactic;

/// <summary>
/// Validates <see cref="UpdateMatchTeamTacticRequest"/> instances.
/// </summary>
public sealed class UpdateMatchTeamTacticValidator : AbstractValidator<UpdateMatchTeamTacticRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateMatchTeamTacticValidator"/> class.
    /// </summary>
    public UpdateMatchTeamTacticValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.MatchId).GreaterThan(0);
        RuleFor(x => x.TeamId).GreaterThan(0);

        // Every enum starts at 1, so IsInEnum also rejects an omitted or default zero.
        RuleFor(x => x.Mentality).IsInEnum();
        RuleFor(x => x.Tempo).IsInEnum();
        RuleFor(x => x.Passing).IsInEnum();
        RuleFor(x => x.Width).IsInEnum();
        RuleFor(x => x.Pressing).IsInEnum();
        RuleFor(x => x.Tackling).IsInEnum();
        RuleFor(x => x.AttackingSide).IsInEnum();

        RuleFor(x => x.PenaltyTakerPlayerId).GreaterThan(0);
        RuleFor(x => x.FreeKickTakerPlayerId).GreaterThan(0);
        RuleFor(x => x.CornerKickTakerPlayerId).GreaterThan(0);
        RuleFor(x => x.CaptainPlayerId).GreaterThan(0);
    }
}
