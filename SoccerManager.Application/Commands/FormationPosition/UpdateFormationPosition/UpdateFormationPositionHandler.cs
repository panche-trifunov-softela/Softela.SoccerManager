using MediatR;
using SoccerManager.Application.Core.User;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Commands.FormationPosition.UpdateFormationPosition;

/// <summary>
/// Handles <see cref="UpdateFormationPositionRequest"/> commands.
/// </summary>
public class UpdateFormationPositionHandler : IRequestHandler<UpdateFormationPositionRequest, bool>
{
    private readonly IFormationPositionRepository _formationPositionRepository;
    private readonly ICurrentUser _currentUser;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateFormationPositionHandler"/> class.
    /// </summary>
    /// <param name="formationPositionRepository">The repository used to load and persist formation positions.</param>
    /// <param name="currentUser">The currently authenticated user.</param>
    /// <param name="unitOfWork">The unit of work used to group the write into one transaction.</param>
    public UpdateFormationPositionHandler(IFormationPositionRepository formationPositionRepository, ICurrentUser currentUser, IUnitOfWork unitOfWork)
    {
        _formationPositionRepository = formationPositionRepository;
        _currentUser = currentUser;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Updates an existing formation position slot with the values from the given request.
    /// </summary>
    /// <param name="request">The update request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns><see langword="true"/> when the update succeeds.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when no formation position with the given identifier exists.</exception>
    public async Task<bool> Handle(UpdateFormationPositionRequest request, CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var formationPosition = await _formationPositionRepository.GetByIdAsync(request.Id)
                ?? throw new KeyNotFoundException($"Formation position {request.Id} not found.");

            var now = DateTime.UtcNow;

            UpdateFormationPositionMapper.ApplyTo(request, formationPosition, now, _currentUser.UserId);

            await _formationPositionRepository.UpdateAsync(formationPosition);

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
