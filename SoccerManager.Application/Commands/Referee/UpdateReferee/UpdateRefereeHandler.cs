using MediatR;
using SoccerManager.Application.Core.User;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Commands.Referee.UpdateReferee;

/// <summary>
/// Handles <see cref="UpdateRefereeRequest"/> commands.
/// </summary>
public class UpdateRefereeHandler : IRequestHandler<UpdateRefereeRequest, bool>
{
    private readonly IRefereeRepository _refereeRepository;
    private readonly ICurrentUser _currentUser;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateRefereeHandler"/> class.
    /// </summary>
    /// <param name="refereeRepository">The repository used to load and persist referees.</param>
    /// <param name="currentUser">The currently authenticated user.</param>
    /// <param name="unitOfWork">The unit of work used to group the write into one transaction.</param>
    public UpdateRefereeHandler(IRefereeRepository refereeRepository, ICurrentUser currentUser, IUnitOfWork unitOfWork)
    {
        _refereeRepository = refereeRepository;
        _currentUser = currentUser;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Updates an existing referee with the values from the given request.
    /// </summary>
    /// <param name="request">The update request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns><see langword="true"/> when the update succeeds.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when no referee with the given identifier exists.</exception>
    public async Task<bool> Handle(UpdateRefereeRequest request, CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var referee = await _refereeRepository.GetByIdAsync(request.Id)
                ?? throw new KeyNotFoundException($"Referee {request.Id} not found.");

            var now = DateTime.UtcNow;

            UpdateRefereeMapper.ApplyTo(request, referee, now, _currentUser.UserId);

            await _refereeRepository.UpdateAsync(referee);

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
