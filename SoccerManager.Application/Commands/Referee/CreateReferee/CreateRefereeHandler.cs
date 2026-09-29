using MediatR;
using SoccerManager.Application.Core.User;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Commands.Referee.CreateReferee;

/// <summary>
/// Handles <see cref="CreateRefereeRequest"/> commands.
/// </summary>
public class CreateRefereeHandler : IRequestHandler<CreateRefereeRequest, int>
{
    private readonly IRefereeRepository _refereeRepository;
    private readonly ICurrentUser _currentUser;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateRefereeHandler"/> class.
    /// </summary>
    /// <param name="refereeRepository">The repository used to persist referees.</param>
    /// <param name="currentUser">The currently authenticated user.</param>
    /// <param name="unitOfWork">The unit of work used to group the write into one transaction.</param>
    public CreateRefereeHandler(IRefereeRepository refereeRepository, ICurrentUser currentUser, IUnitOfWork unitOfWork)
    {
        _refereeRepository = refereeRepository;
        _currentUser = currentUser;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Creates a new referee from the given request.
    /// </summary>
    /// <param name="request">The create request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The identifier of the created referee.</returns>
    public async Task<int> Handle(CreateRefereeRequest request, CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;

        var referee = CreateRefereeMapper.ToDomainEntity(request, now, _currentUser.UserId);

        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var id = await _refereeRepository.CreateAsync(referee);

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
