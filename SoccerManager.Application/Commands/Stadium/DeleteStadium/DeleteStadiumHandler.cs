using MediatR;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Commands.Stadium.DeleteStadium;

/// <summary>
/// Handles <see cref="DeleteStadiumRequest"/> commands. This performs a hard delete, as the
/// entity carries no soft-delete flag.
/// </summary>
public class DeleteStadiumHandler : IRequestHandler<DeleteStadiumRequest, bool>
{
    private readonly IStadiumRepository _stadiumRepository;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteStadiumHandler"/> class.
    /// </summary>
    /// <param name="stadiumRepository">The repository used to load and delete stadiums.</param>
    /// <param name="unitOfWork">The unit of work used to group the write into one transaction.</param>
    public DeleteStadiumHandler(IStadiumRepository stadiumRepository, IUnitOfWork unitOfWork)
    {
        _stadiumRepository = stadiumRepository;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Deletes the stadium identified by the given request.
    /// </summary>
    /// <param name="request">The delete request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns><see langword="true"/> when the deletion succeeds.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when no stadium with the given identifier exists.</exception>
    public async Task<bool> Handle(DeleteStadiumRequest request, CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            _ = await _stadiumRepository.GetByIdAsync(request.Id)
                ?? throw new KeyNotFoundException($"Stadium {request.Id} not found.");

            await _stadiumRepository.DeleteAsync(request.Id);

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
