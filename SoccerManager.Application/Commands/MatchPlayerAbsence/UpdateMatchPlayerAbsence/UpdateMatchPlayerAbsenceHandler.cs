using FluentValidation;
using MediatR;
using SoccerManager.Application.Core.User;
using SoccerManager.Application.Repositories;
using SoccerManager.Application.Services;

namespace SoccerManager.Application.Commands.MatchPlayerAbsence.UpdateMatchPlayerAbsence;

/// <summary>
/// Handles <see cref="UpdateMatchPlayerAbsenceRequest"/> commands.
/// </summary>
public class UpdateMatchPlayerAbsenceHandler : IRequestHandler<UpdateMatchPlayerAbsenceRequest, bool>
{
    private readonly IMatchPlayerAbsenceRepository _matchPlayerAbsenceRepository;
    private readonly IMatchPlayerAbsenceService _matchPlayerAbsenceService;
    private readonly ICurrentUser _currentUser;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateMatchPlayerAbsenceHandler"/> class.
    /// </summary>
    /// <param name="matchPlayerAbsenceRepository">The repository used to load and persist match player absences.</param>
    /// <param name="matchPlayerAbsenceService">The service used to check the rules an absence must keep.</param>
    /// <param name="currentUser">The currently authenticated user.</param>
    /// <param name="unitOfWork">The unit of work used to group the write into one transaction.</param>
    public UpdateMatchPlayerAbsenceHandler(IMatchPlayerAbsenceRepository matchPlayerAbsenceRepository, IMatchPlayerAbsenceService matchPlayerAbsenceService, ICurrentUser currentUser, IUnitOfWork unitOfWork)
    {
        _matchPlayerAbsenceRepository = matchPlayerAbsenceRepository;
        _matchPlayerAbsenceService = matchPlayerAbsenceService;
        _currentUser = currentUser;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Updates an existing match player absence with the values from the given request.
    /// </summary>
    /// <param name="request">The update request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns><see langword="true"/> when the update succeeds.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when no match player absence with the given identifier exists, or the match does not exist.</exception>
    /// <exception cref="ValidationException">Thrown when the absence moves to a club that does not play in the match, a player who is not in the club's squad, a player who already has an absence in the match, or a player who played in it.</exception>
    public async Task<bool> Handle(UpdateMatchPlayerAbsenceRequest request, CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var matchPlayerAbsence = await _matchPlayerAbsenceRepository.GetByIdAsync(request.Id)
                ?? throw new KeyNotFoundException($"Match player absence {request.Id} not found.");

            // An absence that keeps its match, club and player is not checked again, so an operator can still correct the reason of an old one.
            if (request.MatchId != matchPlayerAbsence.MatchId
                || request.LeagueTeamId != matchPlayerAbsence.LeagueTeamId
                || request.PlayerId != matchPlayerAbsence.PlayerId)
            {
                await _matchPlayerAbsenceService.EnsureAbsenceIsAllowedAsync(request.MatchId, request.LeagueTeamId, request.PlayerId, request.Id);
            }

            var now = DateTime.UtcNow;

            UpdateMatchPlayerAbsenceMapper.ApplyTo(request, matchPlayerAbsence, now, _currentUser.UserId);

            await _matchPlayerAbsenceRepository.UpdateAsync(matchPlayerAbsence);

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
