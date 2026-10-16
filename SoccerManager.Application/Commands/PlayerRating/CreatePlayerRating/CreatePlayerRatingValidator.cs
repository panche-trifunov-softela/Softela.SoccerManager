using FluentValidation;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Commands.PlayerRating.CreatePlayerRating;

/// <summary>
/// Validates <see cref="CreatePlayerRatingRequest"/> instances.
/// </summary>
public sealed class CreatePlayerRatingValidator : AbstractValidator<CreatePlayerRatingRequest>
{
    private readonly IPlayerRepository _playerRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreatePlayerRatingValidator"/> class.
    /// </summary>
    /// <param name="playerRepository">The repository used to check the player exists.</param>
    public CreatePlayerRatingValidator(IPlayerRepository playerRepository)
    {
        _playerRepository = playerRepository;

        RuleFor(x => x.PlayerId).GreaterThan(0);
        RuleFor(x => x.Rating).InclusiveBetween(1, 100);

        // The player is only looked up once the id is usable. This runs outside the handler's transaction, so
        // FK_PlayerRatings_Players is the real guarantee; this only turns an unknown player into a 400.
        When(x => x.PlayerId > 0, () =>
        {
            RuleFor(x => x.PlayerId)
                .MustAsync(PlayerExistsAsync)
                .WithMessage("The player does not exist.");
        });
    }

    /// <summary>
    /// Checks that the player exists.
    /// </summary>
    /// <param name="playerId">The identifier of the player being rated.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns><see langword="true"/> when the player exists.</returns>
    private async Task<bool> PlayerExistsAsync(int playerId, CancellationToken cancellationToken)
    {
        return await _playerRepository.GetByIdAsync(playerId) is not null;
    }
}
