using MediatR;
using SoccerManager.Application.Core.User;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Commands.Position.CreatePosition;

/// <summary>
/// Handles <see cref="CreatePositionRequest"/> commands.
/// </summary>
public class CreatePositionHandler : IRequestHandler<CreatePositionRequest, int>
{
    private readonly IPositionRepository _positionRepository;
    private readonly ICurrentUser _currentUser;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreatePositionHandler"/> class.
    /// </summary>
    /// <param name="positionRepository">The repository used to persist positions.</param>
    /// <param name="currentUser">The currently authenticated user.</param>
    /// <param name="unitOfWork">The unit of work used to group the write into one transaction.</param>
    public CreatePositionHandler(IPositionRepository positionRepository, ICurrentUser currentUser, IUnitOfWork unitOfWork)
    {
        _positionRepository = positionRepository;
        _currentUser = currentUser;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Creates a new position from the given request.
    /// </summary>
    /// <param name="request">The create request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The identifier of the created position.</returns>
    public async Task<int> Handle(CreatePositionRequest request, CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;

        var position = CreatePositionMapper.ToDomainEntity(request, now, _currentUser.UserId);

        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var id = await _positionRepository.CreateAsync(position);

            await _unitOfWork.CommitAsync(cancellationToken);

            return id;
        }
        catch
        {
            await _unitOfWork.RollbackAsync(cancellationToken);
            throw;
        }
    }
}
