using MediatR;
using SoccerManager.Application.Core.User;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Commands.Match.UpdateMatch;

/// <summary>
/// Handles <see cref="UpdateMatchRequest"/> commands.
/// </summary>
public class UpdateMatchHandler : IRequestHandler<UpdateMatchRequest, bool>
{
    private readonly IMatchRepository _matchRepository;
    private readonly ICurrentUser _currentUser;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateMatchHandler"/> class.
    /// </summary>
    /// <param name="matchRepository">The repository used to load and persist matches.</param>
    /// <param name="currentUser">The currently authenticated user.</param>
    /// <param name="unitOfWork">The unit of work used to group the write into one transaction.</param>
    public UpdateMatchHandler(IMatchRepository matchRepository, ICurrentUser currentUser, IUnitOfWork unitOfWork)
    {
        _matchRepository = matchRepository;
        _currentUser = currentUser;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Updates an existing match with the values from the given request.
    /// </summary>
    /// <param name="request">The update request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns><see langword="true"/> when the update succeeds.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when no match with the given identifier exists.</exception>
    public async Task<bool> Handle(UpdateMatchRequest request, CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var match = await _matchRepository.GetByIdAsync(request.Id)
                ?? throw new KeyNotFoundException($"Match {request.Id} not found.");

            var now = DateTime.UtcNow;

            UpdateMatchMapper.ApplyTo(request, match, now, _currentUser.UserId);

            await _matchRepository.UpdateAsync(match);

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
