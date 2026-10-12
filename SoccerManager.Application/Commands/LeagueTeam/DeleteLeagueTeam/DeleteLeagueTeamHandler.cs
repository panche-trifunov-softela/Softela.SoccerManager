using MediatR;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Commands.LeagueTeam.DeleteLeagueTeam;

/// <summary>
/// Handles <see cref="DeleteLeagueTeamRequest"/> commands. This performs a hard delete, as the
/// entity carries no soft-delete flag. Only the roster entry is removed: the team's squad,
/// appointments and applications in that league are untouched.
/// </summary>
public class DeleteLeagueTeamHandler : IRequestHandler<DeleteLeagueTeamRequest, bool>
{
    private readonly ILeagueTeamRepository _leagueTeamRepository;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteLeagueTeamHandler"/> class.
    /// </summary>
    /// <param name="leagueTeamRepository">The repository used to load and delete league teams.</param>
    /// <param name="unitOfWork">The unit of work used to group the write into one transaction.</param>
    public DeleteLeagueTeamHandler(ILeagueTeamRepository leagueTeamRepository, IUnitOfWork unitOfWork)
    {
        _leagueTeamRepository = leagueTeamRepository;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Deletes the league team identified by the given request.
    /// </summary>
    /// <param name="request">The delete request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns><see langword="true"/> when the deletion succeeds.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when no league team with the given identifier exists.</exception>
    public async Task<bool> Handle(DeleteLeagueTeamRequest request, CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            _ = await _leagueTeamRepository.GetByIdAsync(request.Id)
                ?? throw new KeyNotFoundException($"LeagueTeam {request.Id} not found.");

            await _leagueTeamRepository.DeleteAsync(request.Id);

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
