using FluentValidation;
using FluentValidation.Results;
using MediatR;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Commands.Season.DeleteSeason;

/// <summary>
/// Handles <see cref="DeleteSeasonRequest"/> commands. This performs a hard delete, as the
/// entity carries no soft-delete flag. The season is removed only when no transfer or loan uses it.
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
    /// <exception cref="ValidationException">Thrown when a transfer or loan still uses the season.</exception>
    public async Task<bool> Handle(DeleteSeasonRequest request, CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            _ = await _seasonRepository.GetByIdAsync(request.Id)
                ?? throw new KeyNotFoundException($"Season {request.Id} not found.");

            var result = await _seasonRepository.DeleteAsync(request.Id);

            if (!result.Deleted)
            {
                if (result.HasTransfers)
                {
                    throw new ValidationException(new[]
                    {
                        new ValidationFailure("Id", "The season cannot be deleted while it still has transfers or loans."),
                    });
                }

                // Nothing flagged means the row vanished between the load above and the delete.
                throw new KeyNotFoundException($"Season {request.Id} not found.");
            }

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
