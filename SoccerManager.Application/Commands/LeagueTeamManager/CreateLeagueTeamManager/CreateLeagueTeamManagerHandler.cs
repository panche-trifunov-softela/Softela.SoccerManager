using MediatR;
using SoccerManager.Application.Core.User;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Commands.LeagueTeamManager.CreateLeagueTeamManager;

/// <summary>
/// Handles <see cref="CreateLeagueTeamManagerRequest"/> commands.
/// </summary>
public class CreateLeagueTeamManagerHandler : IRequestHandler<CreateLeagueTeamManagerRequest, int>
{
    private readonly ILeagueTeamManagerRepository _leagueTeamManagerRepository;
    private readonly ICurrentUser _currentUser;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateLeagueTeamManagerHandler"/> class.
    /// </summary>
    /// <param name="leagueTeamManagerRepository">The repository used to persist league team managers.</param>
    /// <param name="currentUser">The currently authenticated user.</param>
    /// <param name="unitOfWork">The unit of work used to group the write into one transaction.</param>
    public CreateLeagueTeamManagerHandler(ILeagueTeamManagerRepository leagueTeamManagerRepository, ICurrentUser currentUser, IUnitOfWork unitOfWork)
    {
        _leagueTeamManagerRepository = leagueTeamManagerRepository;
        _currentUser = currentUser;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Creates a new league team manager appointment from the given request.
    /// </summary>
    /// <param name="request">The create request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The identifier of the created league team manager.</returns>
    public async Task<int> Handle(CreateLeagueTeamManagerRequest request, CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;

        var leagueTeamManager = CreateLeagueTeamManagerMapper.ToDomainEntity(request, now, _currentUser.UserId);

        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var id = await _leagueTeamManagerRepository.CreateAsync(leagueTeamManager);

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
