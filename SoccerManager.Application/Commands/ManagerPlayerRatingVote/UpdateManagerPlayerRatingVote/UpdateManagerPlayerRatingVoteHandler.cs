using MediatR;
using SoccerManager.Application.Core.User;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Commands.ManagerPlayerRatingVote.UpdateManagerPlayerRatingVote;

/// <summary>
/// Handles <see cref="UpdateManagerPlayerRatingVoteRequest"/> commands.
/// </summary>
public class UpdateManagerPlayerRatingVoteHandler : IRequestHandler<UpdateManagerPlayerRatingVoteRequest, bool>
{
    private readonly IManagerPlayerRatingVoteRepository _managerPlayerRatingVoteRepository;
    private readonly IManagerRepository _managerRepository;
    private readonly ICurrentUser _currentUser;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateManagerPlayerRatingVoteHandler"/> class.
    /// </summary>
    /// <param name="managerPlayerRatingVoteRepository">The repository used to load and persist votes.</param>
    /// <param name="managerRepository">The repository used to find the caller's manager profile.</param>
    /// <param name="currentUser">The currently authenticated user.</param>
    /// <param name="unitOfWork">The unit of work used to group the write into one transaction.</param>
    public UpdateManagerPlayerRatingVoteHandler(IManagerPlayerRatingVoteRepository managerPlayerRatingVoteRepository, IManagerRepository managerRepository, ICurrentUser currentUser, IUnitOfWork unitOfWork)
    {
        _managerPlayerRatingVoteRepository = managerPlayerRatingVoteRepository;
        _managerRepository = managerRepository;
        _currentUser = currentUser;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Changes the direction of one of the caller's votes.
    /// </summary>
    /// <param name="request">The update request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns><see langword="true"/> when the update succeeds.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when the vote does not exist or is not the caller's.</exception>
    public async Task<bool> Handle(UpdateManagerPlayerRatingVoteRequest request, CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            // Only the caller's own vote can change. Someone else's vote, and any vote when the caller has no
            // manager profile, is reported as not found, the same as a missing one.
            var manager = await _managerRepository.GetByUserIdAsync(_currentUser.UserId);
            var managerPlayerRatingVote = await _managerPlayerRatingVoteRepository.GetByIdAsync(request.Id);

            if (managerPlayerRatingVote is null || manager is null || managerPlayerRatingVote.ManagerId != manager.Id)
            {
                throw new KeyNotFoundException($"Manager player rating vote {request.Id} not found.");
            }

            var now = DateTime.UtcNow;

            UpdateManagerPlayerRatingVoteMapper.ApplyTo(request, managerPlayerRatingVote, now, _currentUser.UserId);

            await _managerPlayerRatingVoteRepository.UpdateAsync(managerPlayerRatingVote);

            await _unitOfWork.CommitAsync(cancellationToken);

            return true;
        }
        catch
        {
            await _unitOfWork.RollbackAsync(cancellationToken);
            throw;
        }
    }
}
