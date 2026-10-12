using MediatR;
using SoccerManager.Application.Core.User;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Commands.LeagueTeam.CreateLeagueTeam;

/// <summary>
/// Handles <see cref="CreateLeagueTeamRequest"/> commands.
/// </summary>
public class CreateLeagueTeamHandler : IRequestHandler<CreateLeagueTeamRequest, int>
{
    private readonly ILeagueTeamRepository _leagueTeamRepository;
    private readonly ICurrentUser _currentUser;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateLeagueTeamHandler"/> class.
    /// </summary>
    /// <param name="leagueTeamRepository">The repository used to persist league teams.</param>
    /// <param name="currentUser">The currently authenticated user.</param>
    /// <param name="unitOfWork">The unit of work used to group the write into one transaction.</param>
    public CreateLeagueTeamHandler(ILeagueTeamRepository leagueTeamRepository, ICurrentUser currentUser, IUnitOfWork unitOfWork)
    {
        _leagueTeamRepository = leagueTeamRepository;
        _currentUser = currentUser;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Adds a team to the roster of a league from the given request.
    /// </summary>
    /// <param name="request">The create request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The identifier of the created league team.</returns>
    public async Task<int> Handle(CreateLeagueTeamRequest request, CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;

        var leagueTeam = CreateLeagueTeamMapper.ToDomainEntity(request, now, _currentUser.UserId);

        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var id = await _leagueTeamRepository.CreateAsync(leagueTeam);

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
