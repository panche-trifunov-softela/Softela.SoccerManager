using MediatR;
using SoccerManager.Application.Core.User;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Commands.Manager.CreateManager;

/// <summary>
/// Handles <see cref="CreateManagerRequest"/> commands.
/// </summary>
public class CreateManagerHandler : IRequestHandler<CreateManagerRequest, int>
{
    private readonly IManagerRepository _managerRepository;
    private readonly ICurrentUser _currentUser;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateManagerHandler"/> class.
    /// </summary>
    /// <param name="managerRepository">The repository used to persist managers.</param>
    /// <param name="currentUser">The currently authenticated user.</param>
    /// <param name="unitOfWork">The unit of work used to group the write into one transaction.</param>
    public CreateManagerHandler(IManagerRepository managerRepository, ICurrentUser currentUser, IUnitOfWork unitOfWork)
    {
        _managerRepository = managerRepository;
        _currentUser = currentUser;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Creates a new manager profile from the given request.
    /// </summary>
    /// <param name="request">The create request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The identifier of the created manager.</returns>
    public async Task<int> Handle(CreateManagerRequest request, CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;

        var manager = CreateManagerMapper.ToDomainEntity(request, now, _currentUser.UserId);

        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var id = await _managerRepository.CreateAsync(manager);

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
