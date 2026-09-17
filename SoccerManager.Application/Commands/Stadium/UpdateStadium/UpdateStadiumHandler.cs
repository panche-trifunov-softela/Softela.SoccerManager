using MediatR;
using SoccerManager.Application.Core.User;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Commands.Stadium.UpdateStadium;

/// <summary>
/// Handles <see cref="UpdateStadiumRequest"/> commands.
/// </summary>
public class UpdateStadiumHandler : IRequestHandler<UpdateStadiumRequest, bool>
{
    private readonly IStadiumRepository _stadiumRepository;
    private readonly ICurrentUser _currentUser;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateStadiumHandler"/> class.
    /// </summary>
    /// <param name="stadiumRepository">The repository used to load and persist stadiums.</param>
    /// <param name="currentUser">The currently authenticated user.</param>
    /// <param name="unitOfWork">The unit of work used to group the write into one transaction.</param>
    public UpdateStadiumHandler(IStadiumRepository stadiumRepository, ICurrentUser currentUser, IUnitOfWork unitOfWork)
    {
        _stadiumRepository = stadiumRepository;
        _currentUser = currentUser;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Updates an existing stadium with the values from the given request.
    /// </summary>
    /// <param name="request">The update request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns><see langword="true"/> when the update succeeds.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when no stadium with the given identifier exists.</exception>
    public async Task<bool> Handle(UpdateStadiumRequest request, CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var stadium = await _stadiumRepository.GetByIdAsync(request.Id)
                ?? throw new KeyNotFoundException($"Stadium {request.Id} not found.");

            var now = DateTime.UtcNow;

            UpdateStadiumMapper.ApplyTo(request, stadium, now, _currentUser.UserId);

            await _stadiumRepository.UpdateAsync(stadium);

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
