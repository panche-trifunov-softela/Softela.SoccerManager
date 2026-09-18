using MediatR;
using SoccerManager.Application.Core.User;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Commands.MatchTeamStatistic.CreateMatchTeamStatistic;

/// <summary>
/// Handles <see cref="CreateMatchTeamStatisticRequest"/> commands.
/// </summary>
public class CreateMatchTeamStatisticHandler : IRequestHandler<CreateMatchTeamStatisticRequest, int>
{
    private readonly IMatchTeamStatisticRepository _matchTeamStatisticRepository;
    private readonly ICurrentUser _currentUser;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateMatchTeamStatisticHandler"/> class.
    /// </summary>
    /// <param name="matchTeamStatisticRepository">The repository used to persist match team statistics.</param>
    /// <param name="currentUser">The currently authenticated user.</param>
    /// <param name="unitOfWork">The unit of work used to group the write into one transaction.</param>
    public CreateMatchTeamStatisticHandler(IMatchTeamStatisticRepository matchTeamStatisticRepository, ICurrentUser currentUser, IUnitOfWork unitOfWork)
    {
        _matchTeamStatisticRepository = matchTeamStatisticRepository;
        _currentUser = currentUser;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Creates a new match team statistic from the given request.
    /// </summary>
    /// <param name="request">The create request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The identifier of the created match team statistic.</returns>
    public async Task<int> Handle(CreateMatchTeamStatisticRequest request, CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;

        var matchTeamStatistic = CreateMatchTeamStatisticMapper.ToDomainEntity(request, now, _currentUser.UserId);

        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var id = await _matchTeamStatisticRepository.CreateAsync(matchTeamStatistic);

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
