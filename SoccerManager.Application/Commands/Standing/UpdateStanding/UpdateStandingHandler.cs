using MediatR;
using SoccerManager.Application.Core.User;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Commands.Standing.UpdateStanding;

/// <summary>
/// Handles <see cref="UpdateStandingRequest"/> commands.
/// </summary>
public class UpdateStandingHandler : IRequestHandler<UpdateStandingRequest, bool>
{
    private readonly IStandingRepository _standingRepository;
    private readonly ICurrentUser _currentUser;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateStandingHandler"/> class.
    /// </summary>
    /// <param name="standingRepository">The repository used to load and persist standings.</param>
    /// <param name="currentUser">The currently authenticated user.</param>
    /// <param name="unitOfWork">The unit of work used to group the write into one transaction.</param>
    public UpdateStandingHandler(IStandingRepository standingRepository, ICurrentUser currentUser, IUnitOfWork unitOfWork)
    {
        _standingRepository = standingRepository;
        _currentUser = currentUser;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Updates an existing standing with the values from the given request.
    /// </summary>
    /// <param name="request">The update request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns><see langword="true"/> when the update succeeds.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when no standing with the given identifier exists.</exception>
    public async Task<bool> Handle(UpdateStandingRequest request, CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var standing = await _standingRepository.GetByIdAsync(request.Id)
                ?? throw new KeyNotFoundException($"Standing {request.Id} not found.");

            var now = DateTime.UtcNow;

            UpdateStandingMapper.ApplyTo(request, standing, now, _currentUser.UserId);

            await _standingRepository.UpdateAsync(standing);

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
