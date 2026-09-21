using FluentValidation;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Commands.Formation.CreateFormation;

/// <summary>
/// Validates <see cref="CreateFormationRequest"/> instances.
/// </summary>
public sealed class CreateFormationValidator : AbstractValidator<CreateFormationRequest>
{
    private readonly IFormationRepository _formationRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateFormationValidator"/> class.
    /// </summary>
    /// <param name="formationRepository">The repository used to check the name is not already taken.</param>
    public CreateFormationValidator(IFormationRepository formationRepository)
    {
        _formationRepository = formationRepository;

        // Cascade.Stop so the database is never queried for a name that already failed the cheap rules.
        RuleFor(x => x.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .MaximumLength(100)
            .MustAsync(BeUniqueNameAsync).WithMessage("A formation named '{PropertyValue}' already exists.");
    }

    /// <summary>
    /// Checks that no formation already carries the given name.
    /// </summary>
    /// <param name="name">The candidate formation name.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns><see langword="true"/> when the name is still free.</returns>
    private async Task<bool> BeUniqueNameAsync(string name, CancellationToken cancellationToken)
    {
        // UQ_Formations_Name is the real guarantee; this only turns a duplicate into a 400 instead of a failed insert.
        return await _formationRepository.GetByNameAsync(name) is null;
    }
}
