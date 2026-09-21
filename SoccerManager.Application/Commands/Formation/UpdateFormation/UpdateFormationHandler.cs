using MediatR;
using SoccerManager.Application.Core.User;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Commands.Formation.UpdateFormation;

/// <summary>
/// Handles <see cref="UpdateFormationRequest"/> commands.
/// </summary>
public class UpdateFormationHandler : IRequestHandler<UpdateFormationRequest, bool>
{
    private readonly IFormationRepository _formationRepository;
    private readonly ICurrentUser _currentUser;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateFormationHandler"/> class.
    /// </summary>
    /// <param name="formationRepository">The repository used to load and persist formations.</param>
    /// <param name="currentUser">The currently authenticated user.</param>
    /// <param name="unitOfWork">The unit of work used to group the write into one transaction.</param>
    public UpdateFormationHandler(IFormationRepository formationRepository, ICurrentUser currentUser, IUnitOfWork unitOfWork)
    {
        _formationRepository = formationRepository;
        _currentUser = currentUser;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Updates an existing formation with the values from the given request.
    /// </summary>
    /// <param name="request">The update request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns><see langword="true"/> when the update succeeds.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when no formation with the given identifier exists.</exception>
    public async Task<bool> Handle(UpdateFormationRequest request, CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var formation = await _formationRepository.GetByIdAsync(request.Id)
                ?? throw new KeyNotFoundException($"Formation {request.Id} not found.");

            var now = DateTime.UtcNow;

            UpdateFormationMapper.ApplyTo(request, formation, now, _currentUser.UserId);

            await _formationRepository.UpdateAsync(formation);

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
