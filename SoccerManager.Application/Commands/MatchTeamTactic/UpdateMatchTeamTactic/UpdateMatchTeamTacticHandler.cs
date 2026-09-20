using MediatR;
using SoccerManager.Application.Core.User;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Commands.MatchTeamTactic.UpdateMatchTeamTactic;

/// <summary>
/// Handles <see cref="UpdateMatchTeamTacticRequest"/> commands.
/// </summary>
public class UpdateMatchTeamTacticHandler : IRequestHandler<UpdateMatchTeamTacticRequest, bool>
{
    private readonly IMatchTeamTacticRepository _matchTeamTacticRepository;
    private readonly ICurrentUser _currentUser;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateMatchTeamTacticHandler"/> class.
    /// </summary>
    /// <param name="matchTeamTacticRepository">The repository used to load and persist match team tactics.</param>
    /// <param name="currentUser">The currently authenticated user.</param>
    /// <param name="unitOfWork">The unit of work used to group the write into one transaction.</param>
    public UpdateMatchTeamTacticHandler(IMatchTeamTacticRepository matchTeamTacticRepository, ICurrentUser currentUser, IUnitOfWork unitOfWork)
    {
        _matchTeamTacticRepository = matchTeamTacticRepository;
        _currentUser = currentUser;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Updates an existing match team tactic with the values from the given request.
    /// </summary>
    /// <param name="request">The update request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns><see langword="true"/> when the update succeeds.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when no match team tactic with the given identifier exists.</exception>
    public async Task<bool> Handle(UpdateMatchTeamTacticRequest request, CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var matchTeamTactic = await _matchTeamTacticRepository.GetByIdAsync(request.Id)
                ?? throw new KeyNotFoundException($"Match team tactic {request.Id} not found.");

            var now = DateTime.UtcNow;

            UpdateMatchTeamTacticMapper.ApplyTo(request, matchTeamTactic, now, _currentUser.UserId);

            await _matchTeamTacticRepository.UpdateAsync(matchTeamTactic);

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
