using MediatR;
using SoccerManager.Application.Core.User;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Commands.MatchTeamStatistic.UpdateMatchTeamStatistic;

/// <summary>
/// Handles <see cref="UpdateMatchTeamStatisticRequest"/> commands.
/// </summary>
public class UpdateMatchTeamStatisticHandler : IRequestHandler<UpdateMatchTeamStatisticRequest, bool>
{
    private readonly IMatchTeamStatisticRepository _matchTeamStatisticRepository;
    private readonly ICurrentUser _currentUser;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateMatchTeamStatisticHandler"/> class.
    /// </summary>
    /// <param name="matchTeamStatisticRepository">The repository used to load and persist match team statistics.</param>
    /// <param name="currentUser">The currently authenticated user.</param>
    /// <param name="unitOfWork">The unit of work used to group the write into one transaction.</param>
    public UpdateMatchTeamStatisticHandler(IMatchTeamStatisticRepository matchTeamStatisticRepository, ICurrentUser currentUser, IUnitOfWork unitOfWork)
    {
        _matchTeamStatisticRepository = matchTeamStatisticRepository;
        _currentUser = currentUser;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Updates an existing match team statistic with the values from the given request.
    /// </summary>
    /// <param name="request">The update request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns><see langword="true"/> when the update succeeds.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when no match team statistic with the given identifier exists.</exception>
    public async Task<bool> Handle(UpdateMatchTeamStatisticRequest request, CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var matchTeamStatistic = await _matchTeamStatisticRepository.GetByIdAsync(request.Id)
                ?? throw new KeyNotFoundException($"Match team statistic {request.Id} not found.");

            var now = DateTime.UtcNow;

            UpdateMatchTeamStatisticMapper.ApplyTo(request, matchTeamStatistic, now, _currentUser.UserId);

            await _matchTeamStatisticRepository.UpdateAsync(matchTeamStatistic);

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
