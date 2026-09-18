using MediatR;
using SoccerManager.Application.Core.User;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Commands.LeagueTeamPlayer.CreateLeagueTeamPlayer;

/// <summary>
/// Handles <see cref="CreateLeagueTeamPlayerRequest"/> commands.
/// </summary>
public class CreateLeagueTeamPlayerHandler : IRequestHandler<CreateLeagueTeamPlayerRequest, int>
{
    private readonly ILeagueTeamPlayerRepository _leagueTeamPlayerRepository;
    private readonly ICurrentUser _currentUser;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateLeagueTeamPlayerHandler"/> class.
    /// </summary>
    /// <param name="leagueTeamPlayerRepository">The repository used to persist league team players.</param>
    /// <param name="currentUser">The currently authenticated user.</param>
    /// <param name="unitOfWork">The unit of work used to group the write into one transaction.</param>
    public CreateLeagueTeamPlayerHandler(ILeagueTeamPlayerRepository leagueTeamPlayerRepository, ICurrentUser currentUser, IUnitOfWork unitOfWork)
    {
        _leagueTeamPlayerRepository = leagueTeamPlayerRepository;
        _currentUser = currentUser;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Creates a new league team player from the given request.
    /// </summary>
    /// <param name="request">The create request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The identifier of the created league team player.</returns>
    public async Task<int> Handle(CreateLeagueTeamPlayerRequest request, CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;

        var leagueTeamPlayer = CreateLeagueTeamPlayerMapper.ToDomainEntity(request, now, _currentUser.UserId);

        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var id = await _leagueTeamPlayerRepository.CreateAsync(leagueTeamPlayer);

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
