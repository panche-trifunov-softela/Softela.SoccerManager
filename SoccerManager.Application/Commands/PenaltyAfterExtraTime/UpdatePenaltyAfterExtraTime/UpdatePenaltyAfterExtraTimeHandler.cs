using FluentValidation;
using MediatR;
using SoccerManager.Application.Core.User;
using SoccerManager.Application.Repositories;
using SoccerManager.Application.Services;

namespace SoccerManager.Application.Commands.PenaltyAfterExtraTime.UpdatePenaltyAfterExtraTime;

/// <summary>
/// Handles <see cref="UpdatePenaltyAfterExtraTimeRequest"/> commands.
/// </summary>
public class UpdatePenaltyAfterExtraTimeHandler : IRequestHandler<UpdatePenaltyAfterExtraTimeRequest, bool>
{
    private readonly IPenaltyAfterExtraTimeRepository _penaltyAfterExtraTimeRepository;
    private readonly IMatchLineupService _matchLineupService;
    private readonly ICurrentUser _currentUser;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdatePenaltyAfterExtraTimeHandler"/> class.
    /// </summary>
    /// <param name="penaltyAfterExtraTimeRepository">The repository used to load and persist penalties after extra time.</param>
    /// <param name="matchLineupService">The service used to check that the club plays the match.</param>
    /// <param name="currentUser">The currently authenticated user.</param>
    /// <param name="unitOfWork">The unit of work used to group the write into one transaction.</param>
    public UpdatePenaltyAfterExtraTimeHandler(IPenaltyAfterExtraTimeRepository penaltyAfterExtraTimeRepository, IMatchLineupService matchLineupService, ICurrentUser currentUser, IUnitOfWork unitOfWork)
    {
        _penaltyAfterExtraTimeRepository = penaltyAfterExtraTimeRepository;
        _matchLineupService = matchLineupService;
        _currentUser = currentUser;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Updates an existing penalty after extra time with the values from the given request.
    /// </summary>
    /// <param name="request">The update request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns><see langword="true"/> when the update succeeds.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when no penalty after extra time with the given identifier exists, or the match does not exist.</exception>
    /// <exception cref="ValidationException">Thrown when the club does not play in the match.</exception>
    public async Task<bool> Handle(UpdatePenaltyAfterExtraTimeRequest request, CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var penaltyAfterExtraTime = await _penaltyAfterExtraTimeRepository.GetByIdAsync(request.Id)
                ?? throw new KeyNotFoundException($"Penalty after extra time {request.Id} not found.");

            // The request can move the kick to another match or club, so the check runs on the new values.
            await _matchLineupService.EnsureClubPlaysMatchAsync(request.MatchId, request.LeagueTeamId);

            var now = DateTime.UtcNow;

            UpdatePenaltyAfterExtraTimeMapper.ApplyTo(request, penaltyAfterExtraTime, now, _currentUser.UserId);

            await _penaltyAfterExtraTimeRepository.UpdateAsync(penaltyAfterExtraTime);

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
