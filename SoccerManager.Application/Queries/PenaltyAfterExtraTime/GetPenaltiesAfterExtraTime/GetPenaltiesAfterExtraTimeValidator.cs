using FluentValidation;

namespace SoccerManager.Application.Queries.PenaltyAfterExtraTime.GetPenaltiesAfterExtraTime;

/// <summary>
/// Validates <see cref="GetPenaltiesAfterExtraTimeRequest"/> instances.
/// </summary>
public sealed class GetPenaltiesAfterExtraTimeValidator : AbstractValidator<GetPenaltiesAfterExtraTimeRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetPenaltiesAfterExtraTimeValidator"/> class.
    /// </summary>
    public GetPenaltiesAfterExtraTimeValidator()
    {
        // This query is always scoped to a match, so a missing or zero
        // MatchId must fail fast here rather than silently returning nothing.
        RuleFor(x => x.MatchId).GreaterThan(0);
    }
}
