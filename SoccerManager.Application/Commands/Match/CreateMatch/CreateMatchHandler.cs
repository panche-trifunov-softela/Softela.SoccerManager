using MediatR;
using SoccerManager.Application.Core.User;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Commands.Match.CreateMatch;

/// <summary>
/// Handles <see cref="CreateMatchRequest"/> commands.
/// </summary>
public class CreateMatchHandler : IRequestHandler<CreateMatchRequest, int>
{
    private readonly IMatchRepository _matchRepository;
    private readonly ICurrentUser _currentUser;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateMatchHandler"/> class.
    /// </summary>
    /// <param name="matchRepository">The repository used to persist matches.</param>
    /// <param name="currentUser">The currently authenticated user.</param>
    /// <param name="unitOfWork">The unit of work used to group the write into one transaction.</param>
    public CreateMatchHandler(IMatchRepository matchRepository, ICurrentUser currentUser, IUnitOfWork unitOfWork)
    {
        _matchRepository = matchRepository;
        _currentUser = currentUser;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Creates a new match from the given request.
    /// </summary>
    /// <param name="request">The create request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The identifier of the created match.</returns>
    public async Task<int> Handle(CreateMatchRequest request, CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;

        var match = CreateMatchMapper.ToDomainEntity(request, now, _currentUser.UserId);

        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var id = await _matchRepository.CreateAsync(match);

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
