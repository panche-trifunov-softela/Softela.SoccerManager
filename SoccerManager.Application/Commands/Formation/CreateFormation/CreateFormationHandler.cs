using MediatR;
using SoccerManager.Application.Core.User;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Commands.Formation.CreateFormation;

/// <summary>
/// Handles <see cref="CreateFormationRequest"/> commands.
/// </summary>
public class CreateFormationHandler : IRequestHandler<CreateFormationRequest, int>
{
    private readonly IFormationRepository _formationRepository;
    private readonly ICurrentUser _currentUser;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateFormationHandler"/> class.
    /// </summary>
    /// <param name="formationRepository">The repository used to persist formations.</param>
    /// <param name="currentUser">The currently authenticated user.</param>
    /// <param name="unitOfWork">The unit of work used to group the write into one transaction.</param>
    public CreateFormationHandler(IFormationRepository formationRepository, ICurrentUser currentUser, IUnitOfWork unitOfWork)
    {
        _formationRepository = formationRepository;
        _currentUser = currentUser;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Creates a new formation from the given request.
    /// </summary>
    /// <param name="request">The create request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The identifier of the created formation.</returns>
    public async Task<int> Handle(CreateFormationRequest request, CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;

        var formation = CreateFormationMapper.ToDomainEntity(request, now, _currentUser.UserId);

        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var id = await _formationRepository.CreateAsync(formation);

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
