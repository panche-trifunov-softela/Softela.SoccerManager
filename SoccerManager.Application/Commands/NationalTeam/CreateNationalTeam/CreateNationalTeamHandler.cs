using MediatR;
using SoccerManager.Application.Core.User;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Commands.NationalTeam.CreateNationalTeam;

/// <summary>
/// Handles <see cref="CreateNationalTeamRequest"/> commands.
/// </summary>
public class CreateNationalTeamHandler : IRequestHandler<CreateNationalTeamRequest, int>
{
    private readonly INationalTeamRepository _nationalTeamRepository;
    private readonly ICurrentUser _currentUser;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateNationalTeamHandler"/> class.
    /// </summary>
    /// <param name="nationalTeamRepository">The repository used to persist national teams.</param>
    /// <param name="currentUser">The currently authenticated user.</param>
    /// <param name="unitOfWork">The unit of work used to group the write into one transaction.</param>
    public CreateNationalTeamHandler(INationalTeamRepository nationalTeamRepository, ICurrentUser currentUser, IUnitOfWork unitOfWork)
    {
        _nationalTeamRepository = nationalTeamRepository;
        _currentUser = currentUser;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Creates a new national team from the given request.
    /// </summary>
    /// <param name="request">The create request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The identifier of the created national team.</returns>
    public async Task<int> Handle(CreateNationalTeamRequest request, CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;

        var nationalTeam = CreateNationalTeamMapper.ToDomainEntity(request, now, _currentUser.UserId);

        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var id = await _nationalTeamRepository.CreateAsync(nationalTeam);

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
