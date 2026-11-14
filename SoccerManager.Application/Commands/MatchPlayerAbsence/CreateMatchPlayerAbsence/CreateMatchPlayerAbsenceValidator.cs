using FluentValidation;

namespace SoccerManager.Application.Commands.MatchPlayerAbsence.CreateMatchPlayerAbsence;

/// <summary>
/// Validates <see cref="CreateMatchPlayerAbsenceRequest"/> instances.
/// </summary>
public sealed class CreateMatchPlayerAbsenceValidator : AbstractValidator<CreateMatchPlayerAbsenceRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateMatchPlayerAbsenceValidator"/> class.
    /// </summary>
    public CreateMatchPlayerAbsenceValidator()
    {
        RuleFor(x => x.MatchId).GreaterThan(0);
        RuleFor(x => x.LeagueTeamId).GreaterThan(0);
        RuleFor(x => x.PlayerId).GreaterThan(0);

        // Mirrors CK_MatchPlayerAbsences_ReasonKnown, so a breach is a 400 rather than a database error.
        RuleFor(x => x.Reason).IsInEnum();
    }
}
