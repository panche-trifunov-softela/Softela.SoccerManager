using FluentValidation;

namespace SoccerManager.Application.Queries.MatchPlayerAbsence.GetMatchPlayerAbsences;

/// <summary>
/// Validates <see cref="GetMatchPlayerAbsencesRequest"/> instances.
/// </summary>
public sealed class GetMatchPlayerAbsencesValidator : AbstractValidator<GetMatchPlayerAbsencesRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetMatchPlayerAbsencesValidator"/> class.
    /// </summary>
    public GetMatchPlayerAbsencesValidator()
    {
        // This query is always scoped to a match, so a missing or zero
        // MatchId must fail fast here rather than silently returning nothing.
        RuleFor(x => x.MatchId).GreaterThan(0);
    }
}
