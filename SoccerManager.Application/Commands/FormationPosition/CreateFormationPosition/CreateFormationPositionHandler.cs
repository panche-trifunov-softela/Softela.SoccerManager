using MediatR;
using SoccerManager.Application.Core.User;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Commands.FormationPosition.CreateFormationPosition;

/// <summary>
/// Handles <see cref="CreateFormationPositionRequest"/> commands.
/// </summary>
public class CreateFormationPositionHandler : IRequestHandler<CreateFormationPositionRequest, int>
{
    private readonly IFormationPositionRepository _formationPositionRepository;
    private readonly ICurrentUser _currentUser;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateFormationPositionHandler"/> class.
    /// </summary>
    /// <param name="formationPositionRepository">The repository used to persist formation positions.</param>
    /// <param name="currentUser">The currently authenticated user.</param>
    /// <param name="unitOfWork">The unit of work used to group the write into one transaction.</param>
    public CreateFormationPositionHandler(IFormationPositionRepository formationPositionRepository, ICurrentUser currentUser, IUnitOfWork unitOfWork)
    {
        _formationPositionRepository = formationPositionRepository;
        _currentUser = currentUser;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Creates a new formation position slot from the given request, placing it on the pitch when the request sends a pitch position.
    /// </summary>
    /// <param name="request">The create request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The identifier of the created formation position.</returns>
    public async Task<int> Handle(CreateFormationPositionRequest request, CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;

        var formationPosition = CreateFormationPositionMapper.ToDomainEntity(request, now, _currentUser.UserId);

        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var id = await _formationPositionRepository.CreateAsync(formationPosition);

            // The insert procedure doesn't know the pitch and leaves both NULL, which is right for a reserve slot; a placed slot gets it here.
            if (formationPosition.PitchX.HasValue || formationPosition.PitchY.HasValue)
            {
                formationPosition.Id = id;

                await _formationPositionRepository.SetPitchAsync(formationPosition);
            }

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
