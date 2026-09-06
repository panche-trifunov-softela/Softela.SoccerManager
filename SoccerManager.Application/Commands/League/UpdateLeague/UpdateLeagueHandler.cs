using MediatR;
using SoccerManager.Application.Core.User;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Commands.League.UpdateLeague;

/// <summary>
/// Handles <see cref="UpdateLeagueRequest"/> commands.
/// </summary>
public class UpdateLeagueHandler : IRequestHandler<UpdateLeagueRequest, bool>
{
    private readonly ILeagueRepository _leagueRepository;
    private readonly ICurrentUser _currentUser;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateLeagueHandler"/> class.
    /// </summary>
    /// <param name="leagueRepository">The repository used to load and persist leagues.</param>
    /// <param name="currentUser">The currently authenticated user.</param>
    /// <param name="unitOfWork">The unit of work used to group the write into one transaction.</param>
    public UpdateLeagueHandler(ILeagueRepository leagueRepository, ICurrentUser currentUser, IUnitOfWork unitOfWork)
    {
        _leagueRepository = leagueRepository;
        _currentUser = currentUser;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Updates an existing league with the values from the given request.
    /// </summary>
    /// <param name="request">The update request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns><see langword="true"/> when the update succeeds.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when no league with the given identifier exists.</exception>
    public async Task<bool> Handle(UpdateLeagueRequest request, CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var league = await _leagueRepository.GetByIdAsync(request.Id)
                ?? throw new KeyNotFoundException($"League {request.Id} not found.");

            var now = DateTime.UtcNow;

            UpdateLeagueMapper.ApplyTo(request, league, now, _currentUser.UserId);

            await _leagueRepository.UpdateAsync(league);

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
