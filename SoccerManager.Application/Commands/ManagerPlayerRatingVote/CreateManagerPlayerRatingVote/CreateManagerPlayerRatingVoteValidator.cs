using FluentValidation;
using SoccerManager.Application.Core.User;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Commands.ManagerPlayerRatingVote.CreateManagerPlayerRatingVote;

/// <summary>
/// Validates <see cref="CreateManagerPlayerRatingVoteRequest"/> instances.
/// </summary>
public sealed class CreateManagerPlayerRatingVoteValidator : AbstractValidator<CreateManagerPlayerRatingVoteRequest>
{
    private readonly IManagerRepository _managerRepository;
    private readonly IPlayerRatingRepository _playerRatingRepository;
    private readonly IManagerPlayerRatingVoteRepository _managerPlayerRatingVoteRepository;
    private readonly ICurrentUser _currentUser;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateManagerPlayerRatingVoteValidator"/> class.
    /// </summary>
    /// <param name="managerRepository">The repository used to check the caller has a manager profile.</param>
    /// <param name="playerRatingRepository">The repository used to find the player's current rating.</param>
    /// <param name="managerPlayerRatingVoteRepository">The repository used to check the caller has not already voted on the player's current rating.</param>
    /// <param name="currentUser">The currently authenticated user.</param>
    public CreateManagerPlayerRatingVoteValidator(IManagerRepository managerRepository, IPlayerRatingRepository playerRatingRepository, IManagerPlayerRatingVoteRepository managerPlayerRatingVoteRepository, ICurrentUser currentUser)
    {
        _managerRepository = managerRepository;
        _playerRatingRepository = playerRatingRepository;
        _managerPlayerRatingVoteRepository = managerPlayerRatingVoteRepository;
        _currentUser = currentUser;

        RuleFor(x => x.PlayerId).GreaterThan(0);

        // These lookups run outside the handler's transaction. If two votes race, the unique key still keeps
        // one vote per manager and player rating, and the losing request fails.
        When(x => x.PlayerId > 0, () =>
        {
            RuleFor(x => x)
                .MustAsync(HaveManagerProfileAsync)
                .OverridePropertyName("Manager")
                .WithMessage("Create a manager profile first.");

            RuleFor(x => x.PlayerId).CustomAsync(ValidatePlayerAsync);
        });
    }

    /// <summary>
    /// Checks that the caller has a manager profile.
    /// </summary>
    /// <param name="request">The request being validated.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns><see langword="true"/> when the caller has a manager profile.</returns>
    private async Task<bool> HaveManagerProfileAsync(CreateManagerPlayerRatingVoteRequest request, CancellationToken cancellationToken)
    {
        // A caller who is not yet a manager is told to create a profile; one is never created on their behalf.
        return await _managerRepository.GetByUserIdAsync(_currentUser.UserId) is not null;
    }

    /// <summary>
    /// Checks that the player exists and that the caller has not yet voted on the player's current rating.
    /// </summary>
    /// <param name="playerId">The player the caller votes on.</param>
    /// <param name="context">The context failures are added to.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    private async Task ValidatePlayerAsync(int playerId, ValidationContext<CreateManagerPlayerRatingVoteRequest> context, CancellationToken cancellationToken)
    {
        // Every player has exactly one current rating, so no current rating means no such player.
        var currentRating = await _playerRatingRepository.GetCurrentByPlayerIdAsync(playerId);

        if (currentRating is null)
        {
            context.AddFailure("The player does not exist.");

            return;
        }

        // A caller with no manager profile is reported by the Manager rule, and has no votes to compare.
        var manager = await _managerRepository.GetByUserIdAsync(_currentUser.UserId);

        if (manager is null)
        {
            return;
        }

        var existingVote = await _managerPlayerRatingVoteRepository.GetByManagerAndPlayerRatingAsync(manager.Id, currentRating.Id);

        if (existingVote is not null)
        {
            context.AddFailure("You have already voted on this player's current rating.");
        }
    }
}
