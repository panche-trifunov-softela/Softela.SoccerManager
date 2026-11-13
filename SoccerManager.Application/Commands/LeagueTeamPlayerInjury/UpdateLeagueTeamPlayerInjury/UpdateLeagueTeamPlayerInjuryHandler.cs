using MediatR;
using SoccerManager.Application.Core.User;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Commands.LeagueTeamPlayerInjury.UpdateLeagueTeamPlayerInjury;

/// <summary>
/// Handles <see cref="UpdateLeagueTeamPlayerInjuryRequest"/> commands.
/// </summary>
public class UpdateLeagueTeamPlayerInjuryHandler : IRequestHandler<UpdateLeagueTeamPlayerInjuryRequest, bool>
{
    private readonly ILeagueTeamPlayerInjuryRepository _leagueTeamPlayerInjuryRepository;
    private readonly ILeagueTeamPlayerRepository _leagueTeamPlayerRepository;
    private readonly ICurrentUser _currentUser;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateLeagueTeamPlayerInjuryHandler"/> class.
    /// </summary>
    /// <param name="leagueTeamPlayerInjuryRepository">The repository used to load and persist injuries.</param>
    /// <param name="leagueTeamPlayerRepository">The repository used to load the squad row an injury moves to.</param>
    /// <param name="currentUser">The currently authenticated user.</param>
    /// <param name="unitOfWork">The unit of work used to group the write into one transaction.</param>
    public UpdateLeagueTeamPlayerInjuryHandler(ILeagueTeamPlayerInjuryRepository leagueTeamPlayerInjuryRepository, ILeagueTeamPlayerRepository leagueTeamPlayerRepository, ICurrentUser currentUser, IUnitOfWork unitOfWork)
    {
        _leagueTeamPlayerInjuryRepository = leagueTeamPlayerInjuryRepository;
        _leagueTeamPlayerRepository = leagueTeamPlayerRepository;
        _currentUser = currentUser;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Updates an existing injury with the values from the given request.
    /// </summary>
    /// <param name="request">The update request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns><see langword="true"/> when the update succeeds.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when no injury with the given identifier exists, or the squad row does not exist.</exception>
    public async Task<bool> Handle(UpdateLeagueTeamPlayerInjuryRequest request, CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var leagueTeamPlayerInjury = await _leagueTeamPlayerInjuryRepository.GetByIdAsync(request.Id)
                ?? throw new KeyNotFoundException($"League team player injury {request.Id} not found.");

            // Only an injury that moves to another squad row needs that row to exist.
            if (request.LeagueTeamPlayerId != leagueTeamPlayerInjury.LeagueTeamPlayerId)
            {
                _ = await _leagueTeamPlayerRepository.GetByIdAsync(request.LeagueTeamPlayerId)
                    ?? throw new KeyNotFoundException($"League team player {request.LeagueTeamPlayerId} not found.");
            }

            var now = DateTime.UtcNow;

            UpdateLeagueTeamPlayerInjuryMapper.ApplyTo(request, leagueTeamPlayerInjury, now, _currentUser.UserId);

            await _leagueTeamPlayerInjuryRepository.UpdateAsync(leagueTeamPlayerInjury);

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
