using FluentValidation;
using SoccerManager.Application.Services;

namespace SoccerManager.Application.Commands.Match.DeleteMatchSubstitution;

/// <summary>
/// Validates <see cref="DeleteMatchSubstitutionRequest"/> instances.
/// </summary>
public sealed class DeleteMatchSubstitutionValidator : AbstractValidator<DeleteMatchSubstitutionRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteMatchSubstitutionValidator"/> class.
    /// </summary>
    public DeleteMatchSubstitutionValidator()
    {
        RuleFor(x => x.MatchId).GreaterThan(0);
        RuleFor(x => x.LeagueTeamId).GreaterThan(0);

        RuleFor(x => x.MinuteAt)
            .Must(minute => MatchSubstitutionLimits.Minutes.Contains(minute))
            .WithMessage($"Minute at must be {MatchSubstitutionLimits.MinutesInWords}.");

        // Situation starts at 1, so IsInEnum also refuses a zero or a missing value.
        RuleFor(x => x.Situation).IsInEnum();
    }
}
