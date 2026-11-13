using MediatR;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Commands.LeagueTeamPlayerInjury.DeleteLeagueTeamPlayerInjury;

/// <summary>
/// Handles <see cref="DeleteLeagueTeamPlayerInjuryRequest"/> commands. This performs a hard delete, as the
/// entity carries no soft-delete flag.
/// </summary>
public class DeleteLeagueTeamPlayerInjuryHandler : IRequestHandler<DeleteLeagueTeamPlayerInjuryRequest, bool>
{
    private readonly ILeagueTeamPlayerInjuryRepository _leagueTeamPlayerInjuryRepository;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteLeagueTeamPlayerInjuryHandler"/> class.
    /// </summary>
    /// <param name="leagueTeamPlayerInjuryRepository">The repository used to load and delete injuries.</param>
    /// <param name="unitOfWork">The unit of work used to group the write into one transaction.</param>
    public DeleteLeagueTeamPlayerInjuryHandler(ILeagueTeamPlayerInjuryRepository leagueTeamPlayerInjuryRepository, IUnitOfWork unitOfWork)
    {
        _leagueTeamPlayerInjuryRepository = leagueTeamPlayerInjuryRepository;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Deletes the injury identified by the given request.
    /// </summary>
    /// <param name="request">The delete request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns><see langword="true"/> when the deletion succeeds.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when no injury with the given identifier exists.</exception>
    public async Task<bool> Handle(DeleteLeagueTeamPlayerInjuryRequest request, CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            _ = await _leagueTeamPlayerInjuryRepository.GetByIdAsync(request.Id)
                ?? throw new KeyNotFoundException($"League team player injury {request.Id} not found.");

            await _leagueTeamPlayerInjuryRepository.DeleteAsync(request.Id);

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
