using MediatR;
using SoccerManager.Application.Core.User;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Commands.MatchPlayerStatistic.CreateMatchPlayerStatistic;

/// <summary>
/// Handles <see cref="CreateMatchPlayerStatisticRequest"/> commands.
/// </summary>
public class CreateMatchPlayerStatisticHandler : IRequestHandler<CreateMatchPlayerStatisticRequest, int>
{
    private readonly IMatchPlayerStatisticRepository _matchPlayerStatisticRepository;
    private readonly ICurrentUser _currentUser;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateMatchPlayerStatisticHandler"/> class.
    /// </summary>
    /// <param name="matchPlayerStatisticRepository">The repository used to persist match player statistics.</param>
    /// <param name="currentUser">The currently authenticated user.</param>
    /// <param name="unitOfWork">The unit of work used to group the write into one transaction.</param>
    public CreateMatchPlayerStatisticHandler(IMatchPlayerStatisticRepository matchPlayerStatisticRepository, ICurrentUser currentUser, IUnitOfWork unitOfWork)
    {
        _matchPlayerStatisticRepository = matchPlayerStatisticRepository;
        _currentUser = currentUser;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Creates a new match player statistic from the given request.
    /// </summary>
    /// <param name="request">The create request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The identifier of the created match player statistic.</returns>
    public async Task<int> Handle(CreateMatchPlayerStatisticRequest request, CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;

        var matchPlayerStatistic = CreateMatchPlayerStatisticMapper.ToDomainEntity(request, now, _currentUser.UserId);

        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var id = await _matchPlayerStatisticRepository.CreateAsync(matchPlayerStatistic);

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
