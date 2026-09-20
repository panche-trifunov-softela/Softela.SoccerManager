using MediatR;
using SoccerManager.Application.Core.User;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Commands.MatchTeamTactic.CreateMatchTeamTactic;

/// <summary>
/// Handles <see cref="CreateMatchTeamTacticRequest"/> commands.
/// </summary>
public class CreateMatchTeamTacticHandler : IRequestHandler<CreateMatchTeamTacticRequest, int>
{
    private readonly IMatchTeamTacticRepository _matchTeamTacticRepository;
    private readonly ICurrentUser _currentUser;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateMatchTeamTacticHandler"/> class.
    /// </summary>
    /// <param name="matchTeamTacticRepository">The repository used to persist match team tactics.</param>
    /// <param name="currentUser">The currently authenticated user.</param>
    /// <param name="unitOfWork">The unit of work used to group the write into one transaction.</param>
    public CreateMatchTeamTacticHandler(IMatchTeamTacticRepository matchTeamTacticRepository, ICurrentUser currentUser, IUnitOfWork unitOfWork)
    {
        _matchTeamTacticRepository = matchTeamTacticRepository;
        _currentUser = currentUser;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Creates a new match team tactic from the given request.
    /// </summary>
    /// <param name="request">The create request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The identifier of the created match team tactic.</returns>
    public async Task<int> Handle(CreateMatchTeamTacticRequest request, CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;

        var matchTeamTactic = CreateMatchTeamTacticMapper.ToDomainEntity(request, now, _currentUser.UserId);

        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var id = await _matchTeamTacticRepository.CreateAsync(matchTeamTactic);

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
