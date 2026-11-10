using FluentValidation;
using MediatR;
using SoccerManager.Application.Core.User;
using SoccerManager.Application.Repositories;
using SoccerManager.Application.Services;

namespace SoccerManager.Application.Commands.PenaltyAfterExtraTime.CreatePenaltyAfterExtraTime;

/// <summary>
/// Handles <see cref="CreatePenaltyAfterExtraTimeRequest"/> commands.
/// </summary>
public class CreatePenaltyAfterExtraTimeHandler : IRequestHandler<CreatePenaltyAfterExtraTimeRequest, int>
{
    private readonly IPenaltyAfterExtraTimeRepository _penaltyAfterExtraTimeRepository;
    private readonly IMatchLineupService _matchLineupService;
    private readonly ICurrentUser _currentUser;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreatePenaltyAfterExtraTimeHandler"/> class.
    /// </summary>
    /// <param name="penaltyAfterExtraTimeRepository">The repository used to persist penalties after extra time.</param>
    /// <param name="matchLineupService">The service used to check that the club plays the match.</param>
    /// <param name="currentUser">The currently authenticated user.</param>
    /// <param name="unitOfWork">The unit of work used to group the write into one transaction.</param>
    public CreatePenaltyAfterExtraTimeHandler(IPenaltyAfterExtraTimeRepository penaltyAfterExtraTimeRepository, IMatchLineupService matchLineupService, ICurrentUser currentUser, IUnitOfWork unitOfWork)
    {
        _penaltyAfterExtraTimeRepository = penaltyAfterExtraTimeRepository;
        _matchLineupService = matchLineupService;
        _currentUser = currentUser;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Creates a new penalty after extra time from the given request.
    /// </summary>
    /// <param name="request">The create request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The identifier of the created penalty after extra time.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when the match does not exist.</exception>
    /// <exception cref="ValidationException">Thrown when the club does not play in the match.</exception>
    public async Task<int> Handle(CreatePenaltyAfterExtraTimeRequest request, CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;

        var penaltyAfterExtraTime = CreatePenaltyAfterExtraTimeMapper.ToDomainEntity(request, now, _currentUser.UserId);

        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            await _matchLineupService.EnsureClubPlaysMatchAsync(request.MatchId, request.LeagueTeamId);

            var id = await _penaltyAfterExtraTimeRepository.CreateAsync(penaltyAfterExtraTime);

            await _unitOfWork.CommitAsync(cancellationToken);

            return id;
        }
        catch
        {
            await _unitOfWork.RollbackAsync(cancellationToken);
            throw;
        }
    }
}
