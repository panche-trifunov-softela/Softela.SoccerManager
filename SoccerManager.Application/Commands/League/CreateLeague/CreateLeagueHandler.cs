using MediatR;
using SoccerManager.Application.Core.User;
using SoccerManager.Application.Repositories;
using SoccerManager.Domain.Enums;

namespace SoccerManager.Application.Commands.League.CreateLeague;

/// <summary>
/// Handles <see cref="CreateLeagueRequest"/> commands: creates the league together with its whole roster
/// (the division clubs and every other team as a guest) and its default squads, in one transaction.
/// </summary>
public class CreateLeagueHandler : IRequestHandler<CreateLeagueRequest, int>
{
    private readonly ILeagueRepository _leagueRepository;
    private readonly ILeagueTeamRepository _leagueTeamRepository;
    private readonly ILeagueTeamPlayerRepository _leagueTeamPlayerRepository;
    private readonly IManagerRepository _managerRepository;
    private readonly ICurrentUser _currentUser;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateLeagueHandler"/> class.
    /// </summary>
    /// <param name="leagueRepository">The repository used to persist leagues.</param>
    /// <param name="leagueTeamRepository">The repository used to persist the league's roster.</param>
    /// <param name="leagueTeamPlayerRepository">The repository used to persist the league's default squads.</param>
    /// <param name="managerRepository">The repository used to find the caller's manager profile.</param>
    /// <param name="currentUser">The currently authenticated user.</param>
    /// <param name="unitOfWork">The unit of work used to group the write into one transaction.</param>
    public CreateLeagueHandler(ILeagueRepository leagueRepository, ILeagueTeamRepository leagueTeamRepository, ILeagueTeamPlayerRepository leagueTeamPlayerRepository, IManagerRepository managerRepository, ICurrentUser currentUser, IUnitOfWork unitOfWork)
    {
        _leagueRepository = leagueRepository;
        _leagueTeamRepository = leagueTeamRepository;
        _leagueTeamPlayerRepository = leagueTeamPlayerRepository;
        _managerRepository = managerRepository;
        _currentUser = currentUser;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Creates a new league from the given request, owned by the caller's manager profile, together with its whole
    /// roster (the division clubs and every other team as a guest) and its default squads, in one transaction.
    /// </summary>
    /// <param name="request">The create request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The identifier of the created league.</returns>
    public async Task<int> Handle(CreateLeagueRequest request, CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;

        // The validator has already checked the caller has a manager profile.
        var manager = await _managerRepository.GetByUserIdAsync(_currentUser.UserId);

        var league = CreateLeagueMapper.ToDomainEntity(request, manager!.Id, now, _currentUser.UserId);

        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var id = await _leagueRepository.CreateAsync(league);

            foreach (var teamId in request.DivisionTeamIds)
            {
                await _leagueTeamRepository.CreateAsync(CreateLeagueMapper.ToDivisionLeagueTeam(id, teamId, now, _currentUser.UserId));
            }

            // The division clubs go first, so that this step adds every other team as a guest.
            await _leagueTeamRepository.CreateGuestsAsync(id, now, _currentUser.UserId);

            await _leagueTeamPlayerRepository.CreateDefaultSquadsAsync(id, request.DivisionSquads == DivisionSquads.Default, now, _currentUser.UserId);

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
