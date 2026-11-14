using FluentValidation;

namespace SoccerManager.Application.Commands.MatchPlayerAbsence.UpdateMatchPlayerAbsence;

/// <summary>
/// Validates <see cref="UpdateMatchPlayerAbsenceRequest"/> instances.
/// </summary>
public sealed class UpdateMatchPlayerAbsenceValidator : AbstractValidator<UpdateMatchPlayerAbsenceRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateMatchPlayerAbsenceValidator"/> class.
    /// </summary>
    public UpdateMatchPlayerAbsenceValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.MatchId).GreaterThan(0);
        RuleFor(x => x.LeagueTeamId).GreaterThan(0);
        RuleFor(x => x.PlayerId).GreaterThan(0);

        // Mirrors CK_MatchPlayerAbsences_ReasonKnown, so a breach is a 400 rather than a database error.
        RuleFor(x => x.Reason).IsInEnum();
    }
}
