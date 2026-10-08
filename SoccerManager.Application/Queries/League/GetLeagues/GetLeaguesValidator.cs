using FluentValidation;

namespace SoccerManager.Application.Queries.League.GetLeagues;

/// <summary>
/// Validates <see cref="GetLeaguesRequest"/> instances.
/// </summary>
public sealed class GetLeaguesValidator : AbstractValidator<GetLeaguesRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetLeaguesValidator"/> class.
    /// </summary>
    public GetLeaguesValidator()
    {
        // Matches the NVARCHAR(100) Name column. The procedure parameter would
        // otherwise cut a longer term silently instead of rejecting it.
        RuleFor(x => x.SearchTerm).MaximumLength(100);
    }
}
