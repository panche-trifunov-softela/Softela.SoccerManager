using FluentValidation;

namespace SoccerManager.Application.Queries.Player.GetPlayerProfile;

/// <summary>
/// Validates <see cref="GetPlayerProfileRequest"/> instances.
/// </summary>
public sealed class GetPlayerProfileValidator : AbstractValidator<GetPlayerProfileRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetPlayerProfileValidator"/> class.
    /// </summary>
    public GetPlayerProfileValidator()
    {
        // A missing, zero or negative Id fails fast here, as a 400, rather than reach the handler, where it would be a 404.
        RuleFor(x => x.Id).GreaterThan(0);
    }
}
