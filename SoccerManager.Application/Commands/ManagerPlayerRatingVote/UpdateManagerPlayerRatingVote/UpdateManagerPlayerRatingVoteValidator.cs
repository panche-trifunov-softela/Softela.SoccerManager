using FluentValidation;

namespace SoccerManager.Application.Commands.ManagerPlayerRatingVote.UpdateManagerPlayerRatingVote;

/// <summary>
/// Validates <see cref="UpdateManagerPlayerRatingVoteRequest"/> instances.
/// </summary>
public sealed class UpdateManagerPlayerRatingVoteValidator : AbstractValidator<UpdateManagerPlayerRatingVoteRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateManagerPlayerRatingVoteValidator"/> class.
    /// </summary>
    public UpdateManagerPlayerRatingVoteValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
    }
}
