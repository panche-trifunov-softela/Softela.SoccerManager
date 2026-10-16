using MediatR;
using SoccerManager.Application.Core.User;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Commands.Manager.EnsureMyManager;

/// <summary>
/// Handles <see cref="EnsureMyManagerRequest"/> commands.
/// </summary>
public class EnsureMyManagerHandler : IRequestHandler<EnsureMyManagerRequest, int>
{
    private readonly IManagerRepository _managerRepository;
    private readonly ICurrentUser _currentUser;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="EnsureMyManagerHandler"/> class.
    /// </summary>
    /// <param name="managerRepository">The repository used to persist managers.</param>
    /// <param name="currentUser">The currently authenticated user.</param>
    /// <param name="unitOfWork">The unit of work used to group the write into one transaction.</param>
    public EnsureMyManagerHandler(IManagerRepository managerRepository, ICurrentUser currentUser, IUnitOfWork unitOfWork)
    {
        _managerRepository = managerRepository;
        _currentUser = currentUser;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Returns the identifier of the caller's manager profile, creating the profile with no image when the caller has none.
    /// </summary>
    /// <param name="request">The ensure request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The identifier of the caller's manager profile, new or existing.</returns>
    public async Task<int> Handle(EnsureMyManagerRequest request, CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;

        var manager = EnsureMyManagerMapper.ToDomainEntity(now, _currentUser.UserId);

        // The transaction keeps the procedure's existence-check lock until its insert, which is what makes two
        // first logins at once create a single profile.
        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var id = await _managerRepository.EnsureAsync(manager);

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
