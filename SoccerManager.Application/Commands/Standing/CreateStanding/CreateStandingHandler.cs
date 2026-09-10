using MediatR;
using SoccerManager.Application.Core.User;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Commands.Standing.CreateStanding;

/// <summary>
/// Handles <see cref="CreateStandingRequest"/> commands.
/// </summary>
public class CreateStandingHandler : IRequestHandler<CreateStandingRequest, int>
{
    private readonly IStandingRepository _standingRepository;
    private readonly ICurrentUser _currentUser;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateStandingHandler"/> class.
    /// </summary>
    /// <param name="standingRepository">The repository used to persist standings.</param>
    /// <param name="currentUser">The currently authenticated user.</param>
    /// <param name="unitOfWork">The unit of work used to group the write into one transaction.</param>
    public CreateStandingHandler(IStandingRepository standingRepository, ICurrentUser currentUser, IUnitOfWork unitOfWork)
    {
        _standingRepository = standingRepository;
        _currentUser = currentUser;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Creates a new standing from the given request.
    /// </summary>
    /// <param name="request">The create request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The identifier of the created standing.</returns>
    public async Task<int> Handle(CreateStandingRequest request, CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;

        var standing = CreateStandingMapper.ToDomainEntity(request, now, _currentUser.UserId);

        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var id = await _standingRepository.CreateAsync(standing);

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
