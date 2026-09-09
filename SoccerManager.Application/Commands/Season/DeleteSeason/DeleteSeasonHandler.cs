using MediatR;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Commands.Season.DeleteSeason;

/// <summary>
/// Handles <see cref="DeleteSeasonRequest"/> commands. This performs a hard delete, as the
/// entity carries no soft-delete flag.
/// </summary>
public class DeleteSeasonHandler : IRequestHandler<DeleteSeasonRequest, bool>
{
    private readonly ISeasonRepository _seasonRepository;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteSeasonHandler"/> class.
    /// </summary>
    /// <param name="seasonRepository">The repository used to load and delete seasons.</param>
    /// <param name="unitOfWork">The unit of work used to group the write into one transaction.</param>
    public DeleteSeasonHandler(ISeasonRepository seasonRepository, IUnitOfWork unitOfWork)
    {
        _seasonRepository = seasonRepository;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Deletes the season identified by the given request.
    /// </summary>
    /// <param name="request">The delete request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns><see langword="true"/> when the deletion succeeds.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when no season with the given identifier exists.</exception>
    public async Task<bool> Handle(DeleteSeasonRequest request, CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            _ = await _seasonRepository.GetByIdAsync(request.Id)
                ?? throw new KeyNotFoundException($"Season {request.Id} not found.");

            await _seasonRepository.DeleteAsync(request.Id);

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
