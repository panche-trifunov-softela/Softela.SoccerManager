using FluentValidation;
using MediatR;
using SoccerManager.Application.Core.User;
using SoccerManager.Application.Repositories;
using SoccerManager.Application.Services;

namespace SoccerManager.Application.Commands.MatchPlayerStatistic.UpdateMatchPlayerStatistic;

/// <summary>
/// Handles <see cref="UpdateMatchPlayerStatisticRequest"/> commands.
/// </summary>
public class UpdateMatchPlayerStatisticHandler : IRequestHandler<UpdateMatchPlayerStatisticRequest, bool>
{
    private readonly IMatchPlayerStatisticRepository _matchPlayerStatisticRepository;
    private readonly IMatchLineupService _matchLineupService;
    private readonly ICurrentUser _currentUser;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateMatchPlayerStatisticHandler"/> class.
    /// </summary>
    /// <param name="matchPlayerStatisticRepository">The repository used to load and persist match player statistics.</param>
    /// <param name="matchLineupService">The service used to check that the club plays the match.</param>
    /// <param name="currentUser">The currently authenticated user.</param>
    /// <param name="unitOfWork">The unit of work used to group the write into one transaction.</param>
    public UpdateMatchPlayerStatisticHandler(IMatchPlayerStatisticRepository matchPlayerStatisticRepository, IMatchLineupService matchLineupService, ICurrentUser currentUser, IUnitOfWork unitOfWork)
    {
        _matchPlayerStatisticRepository = matchPlayerStatisticRepository;
        _matchLineupService = matchLineupService;
        _currentUser = currentUser;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Updates an existing match player statistic with the values from the given request.
    /// </summary>
    /// <param name="request">The update request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns><see langword="true"/> when the update succeeds.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when no match player statistic with the given identifier exists, or the match does not exist.</exception>
    /// <exception cref="ValidationException">Thrown when the club does not play in the match.</exception>
    public async Task<bool> Handle(UpdateMatchPlayerStatisticRequest request, CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var matchPlayerStatistic = await _matchPlayerStatisticRepository.GetByIdAsync(request.Id)
                ?? throw new KeyNotFoundException($"Match player statistic {request.Id} not found.");

            await _matchLineupService.EnsureClubPlaysMatchAsync(request.MatchId, request.LeagueTeamId);

            var now = DateTime.UtcNow;

            UpdateMatchPlayerStatisticMapper.ApplyTo(request, matchPlayerStatistic, now, _currentUser.UserId);

            await _matchPlayerStatisticRepository.UpdateAsync(matchPlayerStatistic);

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
