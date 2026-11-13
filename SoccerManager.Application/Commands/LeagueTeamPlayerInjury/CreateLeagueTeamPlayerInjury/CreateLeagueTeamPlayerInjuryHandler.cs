using MediatR;
using SoccerManager.Application.Core.User;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Commands.LeagueTeamPlayerInjury.CreateLeagueTeamPlayerInjury;

/// <summary>
/// Handles <see cref="CreateLeagueTeamPlayerInjuryRequest"/> commands.
/// </summary>
public class CreateLeagueTeamPlayerInjuryHandler : IRequestHandler<CreateLeagueTeamPlayerInjuryRequest, int>
{
    private readonly ILeagueTeamPlayerInjuryRepository _leagueTeamPlayerInjuryRepository;
    private readonly ILeagueTeamPlayerRepository _leagueTeamPlayerRepository;
    private readonly ICurrentUser _currentUser;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateLeagueTeamPlayerInjuryHandler"/> class.
    /// </summary>
    /// <param name="leagueTeamPlayerInjuryRepository">The repository used to persist injuries.</param>
    /// <param name="leagueTeamPlayerRepository">The repository used to load the squad row the injury is recorded on.</param>
    /// <param name="currentUser">The currently authenticated user.</param>
    /// <param name="unitOfWork">The unit of work used to group the write into one transaction.</param>
    public CreateLeagueTeamPlayerInjuryHandler(ILeagueTeamPlayerInjuryRepository leagueTeamPlayerInjuryRepository, ILeagueTeamPlayerRepository leagueTeamPlayerRepository, ICurrentUser currentUser, IUnitOfWork unitOfWork)
    {
        _leagueTeamPlayerInjuryRepository = leagueTeamPlayerInjuryRepository;
        _leagueTeamPlayerRepository = leagueTeamPlayerRepository;
        _currentUser = currentUser;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Creates a new injury from the given request.
    /// </summary>
    /// <param name="request">The create request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The identifier of the created injury.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when the squad row does not exist.</exception>
    public async Task<int> Handle(CreateLeagueTeamPlayerInjuryRequest request, CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            _ = await _leagueTeamPlayerRepository.GetByIdAsync(request.LeagueTeamPlayerId)
                ?? throw new KeyNotFoundException($"League team player {request.LeagueTeamPlayerId} not found.");

            var now = DateTime.UtcNow;

            var leagueTeamPlayerInjury = CreateLeagueTeamPlayerInjuryMapper.ToDomainEntity(request, now, _currentUser.UserId);

            var id = await _leagueTeamPlayerInjuryRepository.CreateAsync(leagueTeamPlayerInjury);

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
