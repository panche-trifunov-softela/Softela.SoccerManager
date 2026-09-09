using MediatR;
using SoccerManager.Application.Core.User;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Commands.Team.UpdateTeam;

/// <summary>
/// Handles <see cref="UpdateTeamRequest"/> commands.
/// </summary>
public class UpdateTeamHandler : IRequestHandler<UpdateTeamRequest, bool>
{
    private readonly ITeamRepository _teamRepository;
    private readonly ICurrentUser _currentUser;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateTeamHandler"/> class.
    /// </summary>
    /// <param name="teamRepository">The repository used to load and persist teams.</param>
    /// <param name="currentUser">The currently authenticated user.</param>
    /// <param name="unitOfWork">The unit of work used to group the write into one transaction.</param>
    public UpdateTeamHandler(ITeamRepository teamRepository, ICurrentUser currentUser, IUnitOfWork unitOfWork)
    {
        _teamRepository = teamRepository;
        _currentUser = currentUser;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Updates an existing team with the values from the given request.
    /// </summary>
    /// <param name="request">The update request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns><see langword="true"/> when the update succeeds.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when no team with the given identifier exists.</exception>
    public async Task<bool> Handle(UpdateTeamRequest request, CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var team = await _teamRepository.GetByIdAsync(request.Id)
                ?? throw new KeyNotFoundException($"Team {request.Id} not found.");

            var now = DateTime.UtcNow;

            UpdateTeamMapper.ApplyTo(request, team, now, _currentUser.UserId);

            await _teamRepository.UpdateAsync(team);

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
