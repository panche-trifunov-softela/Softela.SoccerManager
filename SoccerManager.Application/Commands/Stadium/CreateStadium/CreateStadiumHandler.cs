using MediatR;
using SoccerManager.Application.Core.User;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Commands.Stadium.CreateStadium;

/// <summary>
/// Handles <see cref="CreateStadiumRequest"/> commands.
/// </summary>
public class CreateStadiumHandler : IRequestHandler<CreateStadiumRequest, int>
{
    private readonly IStadiumRepository _stadiumRepository;
    private readonly ICurrentUser _currentUser;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateStadiumHandler"/> class.
    /// </summary>
    /// <param name="stadiumRepository">The repository used to persist stadiums.</param>
    /// <param name="currentUser">The currently authenticated user.</param>
    /// <param name="unitOfWork">The unit of work used to group the write into one transaction.</param>
    public CreateStadiumHandler(IStadiumRepository stadiumRepository, ICurrentUser currentUser, IUnitOfWork unitOfWork)
    {
        _stadiumRepository = stadiumRepository;
        _currentUser = currentUser;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Creates a new stadium from the given request.
    /// </summary>
    /// <param name="request">The create request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The identifier of the created stadium.</returns>
    public async Task<int> Handle(CreateStadiumRequest request, CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;

        var stadium = CreateStadiumMapper.ToDomainEntity(request, now, _currentUser.UserId);

        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var id = await _stadiumRepository.CreateAsync(stadium);

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
