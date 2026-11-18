using MediatR;
using SoccerManager.Application.Core.User;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Commands.NationalTeamMatch.CreateNationalTeamMatch;

/// <summary>
/// Handles <see cref="CreateNationalTeamMatchRequest"/> commands.
/// </summary>
public class CreateNationalTeamMatchHandler : IRequestHandler<CreateNationalTeamMatchRequest, int>
{
    private readonly INationalTeamMatchRepository _nationalTeamMatchRepository;
    private readonly ICurrentUser _currentUser;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateNationalTeamMatchHandler"/> class.
    /// </summary>
    /// <param name="nationalTeamMatchRepository">The repository used to persist national team matches.</param>
    /// <param name="currentUser">The currently authenticated user.</param>
    /// <param name="unitOfWork">The unit of work used to group the write into one transaction.</param>
    public CreateNationalTeamMatchHandler(INationalTeamMatchRepository nationalTeamMatchRepository, ICurrentUser currentUser, IUnitOfWork unitOfWork)
    {
        _nationalTeamMatchRepository = nationalTeamMatchRepository;
        _currentUser = currentUser;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Creates a new national team match from the given request.
    /// </summary>
    /// <param name="request">The create request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The identifier of the created national team match.</returns>
    public async Task<int> Handle(CreateNationalTeamMatchRequest request, CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;

        var nationalTeamMatch = CreateNationalTeamMatchMapper.ToDomainEntity(request, now, _currentUser.UserId);

        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var id = await _nationalTeamMatchRepository.CreateAsync(nationalTeamMatch);

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
