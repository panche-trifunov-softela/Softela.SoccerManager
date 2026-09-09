using MediatR;
using SoccerManager.Application.Core.User;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Commands.Position.UpdatePosition;

/// <summary>
/// Handles <see cref="UpdatePositionRequest"/> commands.
/// </summary>
public class UpdatePositionHandler : IRequestHandler<UpdatePositionRequest, bool>
{
    private readonly IPositionRepository _positionRepository;
    private readonly ICurrentUser _currentUser;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdatePositionHandler"/> class.
    /// </summary>
    /// <param name="positionRepository">The repository used to load and persist positions.</param>
    /// <param name="currentUser">The currently authenticated user.</param>
    /// <param name="unitOfWork">The unit of work used to group the write into one transaction.</param>
    public UpdatePositionHandler(IPositionRepository positionRepository, ICurrentUser currentUser, IUnitOfWork unitOfWork)
    {
        _positionRepository = positionRepository;
        _currentUser = currentUser;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Updates an existing position with the values from the given request.
    /// </summary>
    /// <param name="request">The update request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns><see langword="true"/> when the update succeeds.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when no position with the given identifier exists.</exception>
    public async Task<bool> Handle(UpdatePositionRequest request, CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var position = await _positionRepository.GetByIdAsync(request.Id)
                ?? throw new KeyNotFoundException($"Position {request.Id} not found.");

            var now = DateTime.UtcNow;

            UpdatePositionMapper.ApplyTo(request, position, now, _currentUser.UserId);

            await _positionRepository.UpdateAsync(position);

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
