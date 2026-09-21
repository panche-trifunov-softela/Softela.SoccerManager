using FluentValidation;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Commands.Formation.UpdateFormation;

/// <summary>
/// Validates <see cref="UpdateFormationRequest"/> instances.
/// </summary>
public sealed class UpdateFormationValidator : AbstractValidator<UpdateFormationRequest>
{
    private readonly IFormationRepository _formationRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateFormationValidator"/> class.
    /// </summary>
    /// <param name="formationRepository">The repository used to check the name is not already taken.</param>
    public UpdateFormationValidator(IFormationRepository formationRepository)
    {
        _formationRepository = formationRepository;

        RuleFor(x => x.Id).GreaterThan(0);

        // Cascade.Stop so the database is never queried for a name that already failed the cheap rules.
        RuleFor(x => x.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .MaximumLength(100)
            .MustAsync(BeUniqueNameAsync).WithMessage("A formation named '{PropertyValue}' already exists.");
    }

    /// <summary>
    /// Checks that no other formation already carries the given name.
    /// </summary>
    /// <param name="request">The update request the name belongs to.</param>
    /// <param name="name">The candidate formation name.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns><see langword="true"/> when the name is still free.</returns>
    private async Task<bool> BeUniqueNameAsync(UpdateFormationRequest request, string name, CancellationToken cancellationToken)
    {
        // UQ_Formations_Name is the real guarantee; this only turns a duplicate into a 400 instead of a failed update.
        var existing = await _formationRepository.GetByNameAsync(name);

        return existing is null || existing.Id == request.Id;
    }
}
