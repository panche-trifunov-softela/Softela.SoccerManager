using FluentValidation;
using MediatR;
using SoccerManager.Application.Core.User;
using SoccerManager.Application.Repositories;
using SoccerManager.Application.Services;

namespace SoccerManager.Application.Commands.MatchPlayerAbsence.CreateMatchPlayerAbsence;

/// <summary>
/// Handles <see cref="CreateMatchPlayerAbsenceRequest"/> commands.
/// </summary>
public class CreateMatchPlayerAbsenceHandler : IRequestHandler<CreateMatchPlayerAbsenceRequest, int>
{
    private readonly IMatchPlayerAbsenceRepository _matchPlayerAbsenceRepository;
    private readonly IMatchPlayerAbsenceService _matchPlayerAbsenceService;
    private readonly ICurrentUser _currentUser;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateMatchPlayerAbsenceHandler"/> class.
    /// </summary>
    /// <param name="matchPlayerAbsenceRepository">The repository used to persist match player absences.</param>
    /// <param name="matchPlayerAbsenceService">The service used to check the rules an absence must keep.</param>
    /// <param name="currentUser">The currently authenticated user.</param>
    /// <param name="unitOfWork">The unit of work used to group the write into one transaction.</param>
    public CreateMatchPlayerAbsenceHandler(IMatchPlayerAbsenceRepository matchPlayerAbsenceRepository, IMatchPlayerAbsenceService matchPlayerAbsenceService, ICurrentUser currentUser, IUnitOfWork unitOfWork)
    {
        _matchPlayerAbsenceRepository = matchPlayerAbsenceRepository;
        _matchPlayerAbsenceService = matchPlayerAbsenceService;
        _currentUser = currentUser;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Creates a new match player absence from the given request.
    /// </summary>
    /// <param name="request">The create request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The identifier of the created match player absence.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when the match does not exist.</exception>
    /// <exception cref="ValidationException">Thrown when the club does not play in the match, the player is not in the club's squad, the player already has an absence in the match, or the player played in it.</exception>
    public async Task<int> Handle(CreateMatchPlayerAbsenceRequest request, CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            await _matchPlayerAbsenceService.EnsureAbsenceIsAllowedAsync(request.MatchId, request.LeagueTeamId, request.PlayerId, null);

            var now = DateTime.UtcNow;

            var matchPlayerAbsence = CreateMatchPlayerAbsenceMapper.ToDomainEntity(request, now, _currentUser.UserId);

            var id = await _matchPlayerAbsenceRepository.CreateAsync(matchPlayerAbsence);

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
