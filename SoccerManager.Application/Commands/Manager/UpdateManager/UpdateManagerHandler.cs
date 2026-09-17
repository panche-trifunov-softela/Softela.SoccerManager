using MediatR;
using SoccerManager.Application.Core.User;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Commands.Manager.UpdateManager;

/// <summary>
/// Handles <see cref="UpdateManagerRequest"/> commands.
/// </summary>
public class UpdateManagerHandler : IRequestHandler<UpdateManagerRequest, bool>
{
    private readonly IManagerRepository _managerRepository;
    private readonly ICurrentUser _currentUser;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateManagerHandler"/> class.
    /// </summary>
    /// <param name="managerRepository">The repository used to load and persist managers.</param>
    /// <param name="currentUser">The currently authenticated user.</param>
    /// <param name="unitOfWork">The unit of work used to group the write into one transaction.</param>
    public UpdateManagerHandler(IManagerRepository managerRepository, ICurrentUser currentUser, IUnitOfWork unitOfWork)
    {
        _managerRepository = managerRepository;
        _currentUser = currentUser;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Updates an existing manager profile with the values from the given request.
    /// </summary>
    /// <param name="request">The update request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns><see langword="true"/> when the update succeeds.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when no manager with the given identifier exists.</exception>
    public async Task<bool> Handle(UpdateManagerRequest request, CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var manager = await _managerRepository.GetByIdAsync(request.Id)
                ?? throw new KeyNotFoundException($"Manager {request.Id} not found.");

            var now = DateTime.UtcNow;

            UpdateManagerMapper.ApplyTo(request, manager, now, _currentUser.UserId);

            await _managerRepository.UpdateAsync(manager);

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
