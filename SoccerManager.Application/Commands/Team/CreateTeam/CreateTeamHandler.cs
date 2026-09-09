using MediatR;
using SoccerManager.Application.Core.User;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Commands.Team.CreateTeam;

/// <summary>
/// Handles <see cref="CreateTeamRequest"/> commands.
/// </summary>
public class CreateTeamHandler : IRequestHandler<CreateTeamRequest, int>
{
    private readonly ITeamRepository _teamRepository;
    private readonly ICurrentUser _currentUser;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateTeamHandler"/> class.
    /// </summary>
    /// <param name="teamRepository">The repository used to persist teams.</param>
    /// <param name="currentUser">The currently authenticated user.</param>
    /// <param name="unitOfWork">The unit of work used to group the write into one transaction.</param>
    public CreateTeamHandler(ITeamRepository teamRepository, ICurrentUser currentUser, IUnitOfWork unitOfWork)
    {
        _teamRepository = teamRepository;
        _currentUser = currentUser;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Creates a new team from the given request.
    /// </summary>
    /// <param name="request">The create request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The identifier of the created team.</returns>
    public async Task<int> Handle(CreateTeamRequest request, CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;

        var team = CreateTeamMapper.ToDomainEntity(request, now, _currentUser.UserId);

        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var id = await _teamRepository.CreateAsync(team);

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
