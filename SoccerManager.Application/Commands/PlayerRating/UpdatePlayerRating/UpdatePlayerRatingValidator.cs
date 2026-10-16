using FluentValidation;

namespace SoccerManager.Application.Commands.PlayerRating.UpdatePlayerRating;

/// <summary>
/// Validates <see cref="UpdatePlayerRatingRequest"/> instances.
/// </summary>
public sealed class UpdatePlayerRatingValidator : AbstractValidator<UpdatePlayerRatingRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdatePlayerRatingValidator"/> class.
    /// </summary>
    public UpdatePlayerRatingValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Rating).InclusiveBetween(1, 100);
    }
}
