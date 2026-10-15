using FluentValidation;

namespace SoccerManager.Application.Queries.ManagerPlayerRatingVote.GetManagerPlayerRatingVotes;

/// <summary>
/// Validates <see cref="GetManagerPlayerRatingVotesRequest"/> instances.
/// </summary>
public sealed class GetManagerPlayerRatingVotesValidator : AbstractValidator<GetManagerPlayerRatingVotesRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetManagerPlayerRatingVotesValidator"/> class.
    /// </summary>
    public GetManagerPlayerRatingVotesValidator()
    {
        // This query is always scoped to a player, so a missing or zero
        // PlayerId must fail fast here rather than silently returning nothing.
        RuleFor(x => x.PlayerId).GreaterThan(0);
    }
}
