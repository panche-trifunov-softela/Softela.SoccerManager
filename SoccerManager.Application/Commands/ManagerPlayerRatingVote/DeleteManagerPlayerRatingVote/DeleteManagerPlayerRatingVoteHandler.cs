using MediatR;
using SoccerManager.Application.Core.User;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Commands.ManagerPlayerRatingVote.DeleteManagerPlayerRatingVote;

/// <summary>
/// Handles <see cref="DeleteManagerPlayerRatingVoteRequest"/> commands. This performs a hard delete, as the
/// entity carries no soft-delete flag.
/// </summary>
public class DeleteManagerPlayerRatingVoteHandler : IRequestHandler<DeleteManagerPlayerRatingVoteRequest, bool>
{
    private readonly IManagerPlayerRatingVoteRepository _managerPlayerRatingVoteRepository;
    private readonly IManagerRepository _managerRepository;
    private readonly ICurrentUser _currentUser;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteManagerPlayerRatingVoteHandler"/> class.
    /// </summary>
    /// <param name="managerPlayerRatingVoteRepository">The repository used to load and delete votes.</param>
    /// <param name="managerRepository">The repository used to find the caller's manager profile.</param>
    /// <param name="currentUser">The currently authenticated user, whose vote ownership is checked.</param>
    /// <param name="unitOfWork">The unit of work used to group the write into one transaction.</param>
    public DeleteManagerPlayerRatingVoteHandler(IManagerPlayerRatingVoteRepository managerPlayerRatingVoteRepository, IManagerRepository managerRepository, ICurrentUser currentUser, IUnitOfWork unitOfWork)
    {
        _managerPlayerRatingVoteRepository = managerPlayerRatingVoteRepository;
        _managerRepository = managerRepository;
        _currentUser = currentUser;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Withdraws one of the caller's votes.
    /// </summary>
    /// <param name="request">The delete request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns><see langword="true"/> when the deletion succeeds.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when the vote does not exist or is not the caller's.</exception>
    public async Task<bool> Handle(DeleteManagerPlayerRatingVoteRequest request, CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            // Only the caller's own vote can be withdrawn. Someone else's vote, and any vote when the caller has no
            // manager profile, is reported as not found, the same as a missing one.
            var manager = await _managerRepository.GetByUserIdAsync(_currentUser.UserId);
            var managerPlayerRatingVote = await _managerPlayerRatingVoteRepository.GetByIdAsync(request.Id);

            if (managerPlayerRatingVote is null || manager is null || managerPlayerRatingVote.ManagerId != manager.Id)
            {
                throw new KeyNotFoundException($"Manager player rating vote {request.Id} not found.");
            }

            await _managerPlayerRatingVoteRepository.DeleteAsync(request.Id);

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
